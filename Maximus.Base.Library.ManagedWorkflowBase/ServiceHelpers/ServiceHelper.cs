using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Maximus.Library.Helpers
{
  public static class ServiceHelper
  {
    public static bool IsCluster(string computerName)
    {
      RegistryKey remoteRegistry = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, ComputerHelper.LocalizeName(computerName));
      RegistryKey clusterKey = remoteRegistry.OpenSubKey("Cluster");
      return clusterKey != null;
    }

    public static bool Is64Bit()
    {
      string processorArchitecture = null;
      try
      {
        processorArchitecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
      }
      catch
      {
        return false;
      }
      if (string.IsNullOrEmpty(processorArchitecture))
        return false;
      if (processorArchitecture.Contains("AMD64"))
        return true;
      return false;
    }

    public static string GetTimeBasedTempFileName(bool useDate, bool useTime, bool useSeconds, bool useMilliseconds, bool useRandom, string prefix = null, string extension = "log", bool useLocalTime = true)
    {
      string fileName = "";
      DateTime now = DateTime.UtcNow;
      if (useLocalTime)
        now = DateTime.Now;
      if (useDate)
        fileName += now.ToString("yyyy-MM-dd");
      if (useTime)
        fileName += (useDate ? "-" : "") + now.ToString("HH-mm");
      if (useSeconds)
        fileName += (useDate || useTime ? "-" : "") + now.ToString("ss");
      if (useMilliseconds)
        fileName += (useDate || useTime || useSeconds ? "-" : "") + now.Millisecond.ToString();
      if (useRandom)
      {
        Random random = new Random();
        fileName += (useDate || useTime || useSeconds || useMilliseconds ? "-" : "") + random.Next().ToString("D4").Substring(0, 3);
      }
      fileName = (prefix ?? "") + fileName + "." + (extension ?? "").TrimStart(new char[] { ' ', '.' });
      return fileName;
    }

    public static DateTime FromUnixTime(uint secondsSince1970)
    {
      DateTime Result = new DateTime(1970, 1, 1, 0, 0, 0, 0);
      return Result.AddSeconds(secondsSince1970);
    }
  }
}