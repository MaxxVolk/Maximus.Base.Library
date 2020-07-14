using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Maximus.Library.Helpers
{
  public enum BinaryPrefix { Byte = 0, KiloByte = 1, MegaByte = 2, GigaByte = 3, TeraByte = 4, PetaByte = 5, ExaByte = 6, ZettaByte = 7, YottaByte = 8 }

  public static class FormattingHelper
  {
    private static string[] decimalPrefixNames = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    private static string[] binaryPrefixNames = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" };

    /// <summary>
    /// Cast input object to string or return empty string if null.
    /// </summary>
    /// <param name="obj">Object to cast.</param>
    /// <returns></returns>
    public static string NullToEmptyString(object obj)
    {
      try { return obj.ToString(); } catch { return string.Empty; }
    }

    /// <summary>
    /// Return empty string if input sting is null.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns></returns>
    public static string NullToEmptyString(string str)
    {
      if (str == null)
        return string.Empty;
      else
        return str;
    }

    /// <summary>
    /// Truncate input string to given length.
    /// </summary>
    /// <param name="InputParam">Original string</param>
    /// <param name="charLimit">String size limit (default 255)</param>
    /// <returns>Limited size string</returns>
    public static string TruncateAt(string InputParam, int charLimit = 255)
    {
      if (InputParam.Length <= charLimit)
        return InputParam;
      else
        return InputParam.Substring(0, charLimit);
    }

    static public string TruncateFromBeginning(string InputParam, int charLimit)
    {
      if (charLimit >= InputParam.Length)
        return InputParam;
      else
        return "..." + InputParam.Substring(InputParam.Length - charLimit + 3);
    }

    /// <summary>
    /// Translate a wildcard sting to a regular expression one.
    /// </summary>
    /// <param name="pattern">Wildcard expression to translate</param>
    /// <returns>Regular expression equivalent of the given wildcard one</returns>
    public static string WildcardToRegex(string pattern, bool fixedStart = true, bool fixedEnd = true)
    {
      return (fixedStart ? "^" : "") + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + (fixedEnd ? "$" : "");
    }

    /// <summary>
    /// Test input string against wildcard pattern.
    /// </summary>
    /// <param name="test">Source string</param>
    /// <param name="pattern">Wildcard pattern</param>
    /// <returns>true if source matches wildcard</returns>
    public static bool MatchWildcard(string test, string pattern)
    {
      return Regex.IsMatch(test, WildcardToRegex(pattern), RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    /// <summary>
    /// Convert byte count for human readable display value, by auto-choosing the best unit.
    /// </summary>
    /// <param name="bytes">Original number by bytes.</param>
    /// <param name="prcisionDigits">Number of decimal fraction digits in output.</param>
    /// <param name="useDecimalPrefixes">Chose between decimal (kilo/KB) or binary (kibi/KiB) prefixes. The actual divider is ALWAYS 1024 regardless the prefix.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatBytes1024(long bytesCount, int prcisionDigits = 2, bool useDecimalPrefixes = true)
    {
      decimal bytes = bytesCount;
      return FormatBytes1024(bytes, prcisionDigits, useDecimalPrefixes);
    }

    /// <summary>
    /// Convert byte count for human readable display value, by auto-choosing the best unit.
    /// </summary>
    /// <param name="bytes">Original number by bytes.</param>
    /// <param name="prcisionDigits">Number of decimal fraction digits in output.</param>
    /// <param name="useDecimalPrefixes">Chose between decimal (kilo/KB) or binary (kibi/KiB) prefixes. The actual divider is ALWAYS 1024 regardless the prefix.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatBytes1024(decimal bytes, int prcisionDigits = 2, bool useDecimalPrefixes = true)
    {
      int magnifierCounter = 0;
      do
      {
        if (bytes < 1024)
          return bytes.ToString($"N{prcisionDigits}") + " " + (useDecimalPrefixes ? decimalPrefixNames[magnifierCounter] : binaryPrefixNames[magnifierCounter]);
        bytes = bytes / 1024;
        magnifierCounter++;
      } while (magnifierCounter < decimalPrefixNames.Length);
      return (bytes * 1024).ToString($"N{prcisionDigits}") + " " + (useDecimalPrefixes ? decimalPrefixNames[magnifierCounter - 1] : binaryPrefixNames[magnifierCounter - 1]);
    }

    public static double ConvertBytes1024(decimal fromBytes, BinaryPrefix fromUnit, BinaryPrefix toUnit)
    {
      int magnifierCounter = fromUnit - toUnit;
      if (magnifierCounter == 0)
        return Convert.ToDouble(fromBytes);
      if (magnifierCounter > 0)
      {
        for (int idx = 0; idx < magnifierCounter; idx++)
          fromBytes = fromBytes * 1024;
        return Convert.ToDouble(fromBytes);
      }
      if (magnifierCounter < 0)
      {
        for (int idx = 0; idx < -magnifierCounter; idx++)
          fromBytes = fromBytes / 1024;
        return Convert.ToDouble(fromBytes);
      }
      throw new OverflowException("You just did the impossible!");
    }

    public static string EnumElements(this object[] valueList, string separator = "; ")
    {
      if (valueList == null)
        return string.Empty;
      if (valueList.Length == 0)
        return string.Empty;
      string Result = "";
      foreach (object value in valueList)
        Result += value.ToString() + separator;
      return Result.Substring(0, Result.Length - separator.Length);
    }

    public static string EnumElements(this Dictionary<string, object> valueDictionary, string itemSeparator = "; ", string namevalueSeparator = ": ")
    {
      if (valueDictionary == null)
        return string.Empty;
      if (valueDictionary.Keys.Count == 0)
        return string.Empty;
      string Result = "";
      foreach (KeyValuePair<string, object> pair in valueDictionary)
        Result += pair.Key.ToString() + namevalueSeparator + pair.Value.ToString() + itemSeparator;
      return Result.Substring(0, Result.Length - itemSeparator.Length);
    }

    public static string EnumElements(this Dictionary<string, double> valueDictionary, string itemSeparator = "; ", string namevalueSeparator = ": ", string doubleFormat = "N2")
    {
      if (valueDictionary == null)
        return string.Empty;
      if (valueDictionary.Keys.Count == 0)
        return string.Empty;
      string Result = "";
      foreach (KeyValuePair<string, double> pair in valueDictionary)
        Result += pair.Key.ToString() + namevalueSeparator + pair.Value.ToString(doubleFormat) + itemSeparator;
      return Result.Substring(0, Result.Length - itemSeparator.Length);
    }

    public static string EnumElements(IEnumerable<object> inputObjects, string separator = "; ")
    {
      if (inputObjects == null)
        return string.Empty;
      if (!inputObjects.Any())
        return string.Empty;
      string Result = "";
      foreach (object obj in inputObjects)
        Result += obj.ToString() + separator;
      return Result.Substring(0, Result.LastIndexOf(separator));
    }
  }
}