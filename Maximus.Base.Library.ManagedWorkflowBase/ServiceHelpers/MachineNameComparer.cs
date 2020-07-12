using System;
using System.Collections.Generic;

namespace Maximus.Library.Helpers
{
  /// <summary>
  /// Compare machine names taking domain suffix in account. If both names are FQDN or NetBIOS names, then literal comparison
  /// applied, otherwise either name reduced to NetBIOS format.
  /// </summary>
  public class MachineNameComparer : IComparer<string>
  {
    public int Compare(string x, string y)
    {
      bool isXfull = (x.IndexOf(".") > 0); // cannot be 0, as ".dnssuffix.name" is invalid.
      bool isYfull = (y.IndexOf(".") > 0);
      if (isXfull && isYfull)
        return StringComparer.OrdinalIgnoreCase.Compare(x, y);
      if (!isXfull && !isYfull)
        return StringComparer.OrdinalIgnoreCase.Compare(x, y);
      if (isXfull)
        return StringComparer.OrdinalIgnoreCase.Compare(x.Substring(0, x.IndexOf(".")), y);
      if (isYfull)
        return StringComparer.OrdinalIgnoreCase.Compare(x, y.Substring(0, y.IndexOf(".")));
      throw new Exception("It's impossible to get here. Suppressing compiler error.");
    }

    public static MachineNameComparer OrdinalIgnoreCase
    {
      get { return new MachineNameComparer(); }
    }
  }
}