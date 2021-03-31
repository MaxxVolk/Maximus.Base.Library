using System;
using System.IO;
using Microsoft.Win32;

namespace Maximus.Library.Helpers
{
  /// <summary>
  /// Set of static helper methods to work with remote and local Registry.
  /// <para>
  /// 
  /// </para>
  /// </summary>
  public static class RegistryHelper
  {
    /// <summary>
    /// Read registry value as <seealso cref="string"/>.
    /// Method doesn't check registry value type and cast all types to <seealso cref="string"/>.
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="fullPath">PowerShell style registry key path and value name in one path string. 
    /// <br><i>Example: HKLM:\Software\Vendor\ValueName</i></br>
    /// <br><b>Note:</b> value names with '\' character are not supported as used for path separator</br>
    /// </param>
    /// <param name="defaultValue">Default value to use if target path doesn't exist. When null, an <seealso cref="IOException"/> will be thrown, if path not found.</param>
    /// <returns>A <seealso cref="string"/> value from Registry.</returns>
    /// <exception cref="IOException">: when no default value is set and path is not found.</exception>
    public static string ReadRegistryString(string computerName, string fullPath, string defaultValue = null)
    {
      return ReadRegistryValue(computerName, fullPath, defaultValue).ToString();
    }

    /// <summary>
    /// Read target REG_DWORD value as Unix time (a number of seconds from 1st Jan 1970).
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="fullPath">PowerShell style registry key path and value name in one path string. 
    /// <br><i>Example: HKLM:\Software\Vendor\ValueName</i></br>
    /// <br><b>Note:</b> value names with '\' character are not supported as used for path separator</br>
    /// </param>
    /// <returns>A <seealso cref="DateTime"/> value converted from REG_DWORD</returns>
    /// <exception cref="IOException">: when no default value is set and path is not found.</exception>
    public static DateTime ReadRegistryUnixTime(string computerName, string fullPath)
    {
      int rawResult = (int)ReadRegistryValue(computerName, fullPath, null);
      uint unixTime = unchecked((uint)rawResult);
      return ServiceHelper.FromUnixTime(unixTime);
    }

    /// <summary>
    /// Read REG_DWORD registry value.
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="fullPath">PowerShell style registry key path and value name in one path string. 
    /// <br><i>Example: HKLM:\Software\Vendor\ValueName</i></br>
    /// <br><b>Note:</b> value names with '\' character are not supported as used for path separator</br>
    /// </param>
    /// <param name="defaultValue">Default value to use if target path doesn't exist. When null, an <seealso cref="IOException"/> will be thrown, if path not found.</param>
    /// <returns>Registry value.</returns>
    /// <exception cref="IOException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static uint ReadRegistryUInt(string computerName, string fullPath, uint? defaultValue = null)
    {
      // it should be like this, if not that complicated, then fail.
      if (defaultValue == null)
      {
        object s = ReadRegistryValue(computerName, fullPath, null);
        if (s is int intS)
          return unchecked((uint)intS);
        throw new InvalidCastException("Target Registry value was not of REG_DWORD type.");
      }
      else
      {
        object d = ReadRegistryValue(computerName, fullPath, defaultValue.Value); // to make default value uint, not uint?
        // .Net read REG_DWORD as int
        if (d.GetType() == typeof(int))
        {
          int x = (int)d;
          return unchecked((uint)x);
        }
        // default result returned as uint
        if (d.GetType() == typeof(uint))
          return (uint)d;
        throw new InvalidCastException("Target Registry value was not of REG_DWORD type.");
      }
    }

    /// <summary>
    /// Read registry value as boolean. Numeric registry types REG_DWORD and REG_QWORD, and REG_SZ are supported.
    /// For numeric values zero is read as <see langword="false"/>, and any non-zero as <see langword="true"/>.
    /// For string values literals 'Yes'/'No' or 'True'/'False' are supported.
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="fullPath">PowerShell style registry key path and value name in one path string. 
    /// <br><i>Example: HKLM:\Software\Vendor\ValueName</i></br>
    /// <br><b>Note:</b> value names with '\' character are not supported as used for path separator</br>
    /// </param>
    /// <param name="defaultValue">Default value to use if target path doesn't exist. When null, an <seealso cref="IOException"/> will be thrown, if path not found.</param>
    /// <returns>Boolean value based on content of target's registry value.</returns>
    public static bool ReadRegistryBoolean(string computerName, string fullPath, bool? defaultValue = null)
    {
      object RawResult;
      if (defaultValue == null)
        RawResult = ReadRegistryValue(computerName, fullPath, null);
      else
        RawResult = ReadRegistryValue(computerName, fullPath, defaultValue.Value ? 1 : 0);

      if (RawResult is int intResult)
        return intResult != 0;
      if (RawResult is long longResult)
        return longResult != 0;
      if (RawResult is string strResult)
        return strResult.ToUpperInvariant() == "TRUE" || strResult.ToUpperInvariant() == "YES";

      throw new InvalidCastException("Target Registry value was not of REG_QWORD, REG_DWORD, not REG_SZ types.");
    }

    /// <summary>
    /// Read REG_QWORD or REG_DWORD registry value.
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="fullPath">PowerShell style registry key path and value name in one path string. 
    /// <br><i>Example: HKLM:\Software\Vendor\ValueName</i></br>
    /// <br><b>Note:</b> value names with '\' character are not supported as used for path separator</br>
    /// </param>
    /// <param name="defaultValue">Default value to use if target path doesn't exist. When null, an <seealso cref="IOException"/> will be thrown, if path not found.</param>
    /// <returns>Registry value.</returns>
    /// <exception cref="IOException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static ulong ReadRegistryULong(string computerName, string fullPath, ulong? defaultValue = null)
    {
      // it should be like this, if not that complicated, then fail.
      if (defaultValue == null)
      {
        object s = ReadRegistryValue(computerName, fullPath, null);
        if (s is int intS)
          return unchecked((ulong)intS);
        if (s is long longS)
          return unchecked((ulong)longS);
        throw new InvalidCastException("Target Registry value was not of REG_QWORD type.");
      }
      else
      {
        object d = ReadRegistryValue(computerName, fullPath, defaultValue.Value); // to make default value ulong, not ulong?
        // .Net read REG_QWORD as long
        if (d is int intS)
          return unchecked((ulong)intS);
        if (d is long longS)
          return unchecked((ulong)longS);
        // default result returned as ulong
        if (d.GetType() == typeof(ulong))
          return (ulong)d;
        throw new InvalidCastException("Target Registry value was not of REG_QWORD type.");
      }
    }

    /// <summary>
    /// Read registry parameter value from full path: HKLM:\Key\Parameter. The PowerShell-style key prefix can be replaced with its
    /// full name: HKEY_LOCAL_MACHINE
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="fullPath">PowerShell style registry key path and value name in one path string. 
    /// <br><i>Example: HKLM:\Software\Vendor\ValueName</i></br>
    /// <br><b>Note:</b> value names with '\' character are not supported as used for path separator</br>
    /// </param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static object ReadRegistryValue(string computerName, string fullPath, object defaultValue = null)
    {
      try
      {
        string pathName = ParseValuePath(fullPath, out string valueName);
        using (RegistryKey valueKey = GetRegistryKey(computerName, pathName))
        {
          if (valueKey == null)
            throw new IOException(pathName + " registry path not found.");
          object returnValue = valueKey.GetValue(valueName);
          if (returnValue == null)
            throw new IOException(valueName + " registry value not found.");
          return returnValue;
        }
      }
      catch (IOException)
      {
        if (defaultValue != null)
          return defaultValue;
        else
          throw;
      }
    }

    /// <summary>
    /// Check registry key existence.
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="keyPath"></param>
    /// <returns>True if the key exists, otherwise false.</returns>
    public static bool RegistryKeyExists(string computerName, string keyPath)
    {
      if (GetRegistryKey(computerName, keyPath) == null)
        return false;
      else
        return true;
    }

    /// <summary>
    /// Check registry value existence.
    /// </summary>
    /// <param name="computerName">Host name of remoter computer. Use '.' or 'localhost' or empty or null string for local Registry access. </param>
    /// <param name="fullPath">PowerShell style registry key path and value name in one path string. 
    /// <br><i>Example: HKLM:\Software\Vendor\ValueName</i></br>
    /// <br><b>Note:</b> value names with '\' character are not supported as used for path separator</br>
    /// </param>
    /// <returns>True if registry value exists, otherwise false.</returns>
    public static bool RegistryValueExists(string computerName, string fullPath)
    {
      try
      {
        ReadRegistryValue(computerName, fullPath);
        return true;
      }
      catch (IOException)
      {
        return false;
      }
    }

    /// <summary>
    /// Creates a new registry key. If the key exists, then returns it.
    /// </summary>
    /// <param name="computerName">Remote computer name, or '.' for local computer.</param>
    /// <param name="keyPath">Full path to the key.</param>
    /// <param name="writable"><see langword="true"/> is existing key should be opened for write. Not applicable for newly created key.</param>
    /// <returns></returns>
    public static RegistryKey CreateRegistryKey(string computerName, string keyPath, bool writable = false)
    {
      RegistryKey draftResult = GetRegistryKey(computerName, keyPath, writable);
      if (draftResult != null)
        return draftResult;
      RegistryHive hKey = ParseKeyPath(keyPath, out string pathName);
      RegistryKey remoteRegistry = RegistryKey.OpenRemoteBaseKey(hKey, ComputerHelper.LocalizeName(computerName));
      if (!string.IsNullOrEmpty(pathName))
      {
        string[] pathParts = pathName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
        RegistryKey cursor = remoteRegistry;
        foreach (string part in pathParts)
        {
          RegistryKey nested = cursor.OpenSubKey(part, true);
          if (nested == null)
            nested = cursor.CreateSubKey(part);
          cursor = nested;
        }
        return cursor;
      }
      else
        return remoteRegistry;
    }

    /// <summary>
    /// Returns <seealso cref="RegistryKey"/> managed object for the referenced key.
    /// </summary>
    /// <param name="computerName"></param>
    /// <param name="keyPath"></param>
    /// <param name="writable"></param>
    /// <returns></returns>
    public static RegistryKey GetRegistryKey(string computerName, string keyPath, bool writable = false)
    {
      RegistryHive hKey = ParseKeyPath(keyPath, out string pathName);
      RegistryKey remoteRegistry = RegistryKey.OpenRemoteBaseKey(hKey, ComputerHelper.LocalizeName(computerName));
      if (string.IsNullOrEmpty(pathName))
        return remoteRegistry;
      else
        return remoteRegistry.OpenSubKey(pathName, writable);
    }

    private static RegistryHive ParseKeyPath(string keyPath, out string relativePath)
    {
      int firstPeriodPos = keyPath.IndexOf("\\");
      string hKeyName = keyPath.Substring(0, firstPeriodPos);
      RegistryHive hKey = RegistryHive.LocalMachine;
      switch (hKeyName.ToUpperInvariant())
      {
        case "HKLM:":
        case "HKLM":
        case "HKEY_LOCAL_MACHINE":
          hKey = RegistryHive.LocalMachine;
          break;
        case "HKCR:":
        case "HKCR":
        case "HKEY_CLASSES_ROOT":
          hKey = RegistryHive.ClassesRoot;
          break;
        case "HKCC:":
        case "HKCC":
        case "HKEY_CURRENT_CONFIG":
          hKey = RegistryHive.CurrentConfig;
          break;
        case "HKCU:":
        case "HKCU":
        case "HKEY_CURRENT_USER":
          hKey = RegistryHive.CurrentUser;
          break;
        case "HKDD:":
        case "HKDD":
        case "HKEY_DYN_DATA":
          hKey = RegistryHive.DynData;
          break;
        case "HKPD:":
        case "HKPD":
        case "HKEY_PERFROMANCE_DATA":
          hKey = RegistryHive.PerformanceData;
          break;
        case "HKU:":
        case "HKU":
        case "HKEY_USERS":
          hKey = RegistryHive.Users;
          break;
      }
      relativePath = keyPath.Substring(firstPeriodPos + 1);
      return hKey;
    }

    private static string ParseValuePath(string fullPath, out string valueName)
    {
      int lastPeriodPos = fullPath.LastIndexOf("\\");
      valueName = fullPath.Substring(lastPeriodPos + 1);
      return fullPath.Substring(0, lastPeriodPos);
    }
  }
}