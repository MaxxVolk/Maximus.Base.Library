using System;
using System.IO;
using Microsoft.Win32;

namespace Maximus.Library.Helpers
{
  public static class RegistryHelper
  {
    public static string ReadRegistryString(string computerName, string fullPath, string defaultValue = null)
    {
      return ReadRegistryValue(computerName, fullPath, defaultValue).ToString();
    }

    public static DateTime ReadRegistryUnixTime(string computerName, string fullPath)
    {
      int rawResult = (int)ReadRegistryValue(computerName, fullPath, null);
      uint unixTime = unchecked((uint)rawResult);
      return ServiceHelper.FromUnixTime(unixTime);
    }

    public static uint ReadRegistryUInt(string computerName, string fullPath, uint? defaultValue = null)
    {
      int rawResult = (int)ReadRegistryValue(computerName, fullPath, defaultValue);
      return unchecked((uint)rawResult);
    }

    public static bool ReadRegistryBoolean(string computerName, string fullPath, bool? defaultValue = null)
    {
      int? defaultInt = null;
      if (defaultValue != null)
        defaultInt = defaultValue == true ? 1 : 0;
      int RawResult = (int)ReadRegistryValue(computerName, fullPath, defaultInt);
      return RawResult != 0;
    }

    public static ulong ReadRegistryULong(string computerName, string fullPath, ulong? defaultValue = null)
    {
      return unchecked((ulong)ReadRegistryValue(computerName, fullPath, defaultValue));
    }

    /// <summary>
    /// Read registry parameter value from full path: HKLM:\Key\Parameter. The PowerShell-style key prefix can be replaced with its
    /// full name: HKEY_LOCAL_MACHINE
    /// </summary>
    /// <param name="computerName">Remote computer name. Leave empty, null, '.' or 'localhost' to access local Registry.</param>
    /// <param name="fullPath"></param>
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

    public static bool RegistryKeyExists(string computerName, string keyPath)
    {
      if (GetRegistryKey(computerName, keyPath) == null)
        return false;
      else
        return true;
    }

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