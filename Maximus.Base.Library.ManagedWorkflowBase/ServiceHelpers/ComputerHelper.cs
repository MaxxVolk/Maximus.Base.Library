using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Maximus.Library.Helpers
{
  [Flags]
  public enum DomainJoinFlags : int
  {
    JOIN_DOMAIN = 1,
    ACCT_CREATE = 2,
    ACCT_DELETE = 4,
    WIN9X_UPGRADE = 16,
    DOMAIN_JOIN_IF_JOINED = 32,
    JOIN_UNSECURE = 64,
    MACHINE_PASSWORD_PASSED = 128,
    DEFERRED_SPN_SET = 256,
    INSTALL_INVOCATION = 262144,
  }

  public static class ComputerHelper
  {
    /// <summary>
    /// Simplify computer name if it is the local computer. Make the string empty (default) or replace with the specified value.
    /// </summary>
    /// <param name="remoteName">Input: computer name to match with the local.</param>
    /// <param name="localReplacement">If the input name is local, then replace with this string, default to empty string.</param>
    /// <returns></returns>
    public static string LocalizeName(string remoteName, string localReplacement = "")
    {
      if (string.IsNullOrEmpty(remoteName) || remoteName == "." || remoteName == string.Empty || remoteName.ToUpperInvariant() == "LOCALHOST")
        return localReplacement;
      if (remoteName.ToUpperInvariant() == Environment.MachineName.ToUpperInvariant())
        return localReplacement;
      if (remoteName.ToUpperInvariant() == Dns.GetHostName().ToUpperInvariant())
        return localReplacement;
      if (remoteName.ToUpperInvariant() == Dns.GetHostName().ToUpperInvariant() + "." + IPGlobalProperties.GetIPGlobalProperties().DomainName.ToUpperInvariant())
        return localReplacement;
      return remoteName;
    }

    public static string GetMachineDNSName()
    {
      string computerName = "";
      string dnsName = Dns.GetHostName();
      if (string.IsNullOrEmpty(dnsName))
        computerName = Environment.MachineName;
      else
      {
        if (dnsName.IndexOf(".") >= 0)
          computerName = dnsName;
        else
        {
          string dnsSuffix = IPGlobalProperties.GetIPGlobalProperties().DomainName;
          computerName = dnsName + (string.IsNullOrEmpty(dnsSuffix) ? "" : $".{dnsSuffix}");
        }
      }
      return computerName;
    }

    public static List<IPAddress> GetMachineIPAddresses(NetworkInterfaceType _type)
    {
      List<IPAddress> ipAddrList = new List<IPAddress>();
      foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
      {
        if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
        {
          foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
          {
            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
            {
              ipAddrList.Add(ip.Address);
            }
          }
        }
      }
      return ipAddrList;
    }

    public static void JoinDomainOrWorkgroup(string domainName, string AccountOU = null, string username = null, string password = null, DomainJoinFlags JoinOptions = DomainJoinFlags.ACCT_CREATE | DomainJoinFlags.JOIN_DOMAIN)
    {
      using (WMIQuery localComputer = new WMIQuery("SELECT * FROM Win32_ComputerSystem"))
        foreach (ManagementBaseObject computer in localComputer.Select())
        {
          ManagementObject computerSystem = (ManagementObject)computer;
          computerSystem.Scope.Options.Authentication = AuthenticationLevel.PacketPrivacy;
          computerSystem.Scope.Options.Impersonation = ImpersonationLevel.Impersonate;
          computerSystem.Scope.Options.EnablePrivileges = true;
          // Execute the method and obtain the return values.
          ManagementBaseObject outParams = computerSystem.InvokeWMIMethod("JoinDomainOrWorkgroup", new Dictionary<string, object>
          {
            { "Name", domainName },
            { "AccountOU", AccountOU },
            { "Password", password },
            { "UserName", username },
            { "FJoinOptions", (int)JoinOptions }
          });
          // Did it work?
          int hResult = Convert.ToInt32((uint)outParams.Properties["ReturnValue"].Value);
          if (hResult != 0)
          {
            switch (hResult)
            {
              case 5:
                throw new Win32Exception(hResult, "Access is denied");
              case 87:
                throw new Win32Exception(hResult, "The parameter is incorrect");
              case 110:
                throw new Win32Exception(hResult, "The system cannot open the specified object");
              case 1323:
                throw new Win32Exception(hResult, "Unable to update the password");
              case 1326:
                throw new Win32Exception(hResult, "Logon failure: unknown username or bad password");
              case 1355:
                throw new Win32Exception(hResult, "The specified domain either does not exist or could not be contacted");
              case 2224:
                throw new Win32Exception(hResult, "The account already exists");
              case 2691:
                throw new Win32Exception(hResult, "The machine is already joined to the domain");
              case 2692:
                throw new Win32Exception(hResult, "The machine is not currently joined to a domain");
              default:
                throw new Win32Exception(hResult);
            }
          }
          break; // there must be only one object always
        }
    }

    public static void SetDNSServerSearchOrder(string[] dnsSearchOrder, string SettingID)
    {
      using (WMIQuery localComputer = new WMIQuery($"SELECT * FROM Win32_NetworkAdapterConfiguration where SettingID = '{SettingID}'"))
        foreach (ManagementBaseObject nic in localComputer.Select())
        {
          ManagementObject networkAdapter = (ManagementObject)nic;
          networkAdapter.Scope.Options.EnablePrivileges = true;
          ManagementBaseObject outParams = networkAdapter.InvokeWMIMethod("SetDNSServerSearchOrder", new Dictionary<string, object>
          {
            { "DNSServerSearchOrder", dnsSearchOrder }
          });
          int hResult = Convert.ToInt32((uint)outParams.Properties["ReturnValue"].Value);
          if (hResult != 0)
          {
            switch (hResult)
            {
              case 1:
                throw new Win32Exception(hResult, "Reboot is required.");
              case 91:
                throw new Win32Exception(hResult, "Access is denied");
              default:
                throw new Win32Exception(hResult);
            }
          }
        }
    }

    public static string GetMachinePrincipalName()
    {
      string Result = ".";
      try
      {
        ManagementScope localWMI = new ManagementScope(ManagementPath.DefaultPath);
        localWMI.Connect();
        ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
        ManagementObjectSearcher computerSystemSearcher = null;
        ManagementObjectCollection computerSystemList = null;
        ManagementObjectCollection.ManagementObjectEnumerator computerSystemEnum = null;
        try
        {
          computerSystemSearcher = new ManagementObjectSearcher(localWMI, query);
          computerSystemList = computerSystemSearcher.Get();
          computerSystemEnum = computerSystemList.GetEnumerator();
          if (computerSystemEnum.MoveNext())
          {
            ManagementBaseObject computerSystem = computerSystemEnum.Current;
            if ((bool)computerSystem["PartOfDomain"])
              Result = computerSystem["DNSHostName"].ToString() + "." + computerSystem["Domain"].ToString();
            else
              Result = computerSystem["DNSHostName"].ToString();
          }
          else
            throw new ArgumentNullException("No instances of Win32_ComputerSystem WMI class returned.");
        }
        finally
        {
          if (computerSystemEnum != null)
            computerSystemEnum.Dispose();
          if (computerSystemList != null)
            computerSystemList.Dispose();
          if (computerSystemSearcher != null)
            computerSystemSearcher.Dispose();
        }
      }
      catch
      {
        Result = Environment.MachineName;
      }
      return Result;
    }

    public static long GetUptime(string computerName)
    {
      ManagementObject mo = new ManagementObject(@"\\" + LocalizeName(computerName, ".") + @"\root\cimv2:Win32_OperatingSystem=@");
      DateTime lastBootUp = ManagementDateTimeConverter.ToDateTime(mo["LastBootUpTime"].ToString());
      return Convert.ToInt64((DateTime.Now.ToUniversalTime() - lastBootUp.ToUniversalTime()).TotalSeconds);
    }

    public static DataTable GetQueryWMI(string computerName, string WQLquery, string WMInamespace = @"\root\cimv2")
    {
      ManagementScope scope = new ManagementScope("\\\\" + LocalizeName(computerName, ".") + WMInamespace);
      scope.Connect();
      if (scope.IsConnected)
      {
        ObjectQuery query = new ObjectQuery(WQLquery);
        ManagementObjectSearcher objectList = null;
        ManagementObjectCollection allResources = null;
        DataTable Result = null;
        try
        {
          objectList = new ManagementObjectSearcher(scope, query);
          allResources = objectList.Get();
          foreach (ManagementBaseObject x in allResources)
          {
            // create output table and use the first row to initialize columns
            if (Result == null)
            {
              
              Result = new DataTable(x.ClassPath.ClassName);
              foreach (var col in x.Properties)
                Result.Columns.Add(col.Name, col.GetManagedType());
            }
            // Add data row
            DataRow newRow = Result.NewRow();
            foreach (var dataCol in x.Properties)
            {
              if (x[dataCol.Name] == null)
              {
                newRow[dataCol.Name] = DBNull.Value;
                continue;
              }
              if (dataCol.Type == CimType.DateTime)
                newRow[dataCol.Name] = ManagementDateTimeConverter.ToDateTime(x[dataCol.Name].ToString());
              else
                newRow[dataCol.Name] = x[dataCol.Name];
            }
            Result.Rows.Add(newRow);
            x.Dispose();
          }
        }
        finally
        {
          if (allResources != null)
            allResources.Dispose();
          if (objectList != null)
            objectList.Dispose();
        }
        return Result;
      }
      return null;
    }
  }
}