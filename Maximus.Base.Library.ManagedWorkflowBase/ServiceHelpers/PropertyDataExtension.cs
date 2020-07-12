using System;
using System.Management;

namespace Maximus.Library.Helpers
{
  public static class PropertyDataExtension
  {
    public static Type GetManagedType(this PropertyData property)
    {
      if (property.IsArray)
      {
        switch (property.Type)
        {
          case CimType.Boolean: return typeof(bool[]);
          case CimType.Char16: return typeof(char[]);
          case CimType.DateTime: return typeof(DateTime[]);
          case CimType.Real32: return typeof(float[]);
          case CimType.Real64: return typeof(double[]);
          case CimType.SInt16: return typeof(short[]);
          case CimType.SInt32: return typeof(int[]);
          case CimType.SInt64: return typeof(long[]);
          case CimType.SInt8: return typeof(sbyte[]);
          case CimType.String: return typeof(string[]);
          case CimType.UInt16: return typeof(ushort[]);
          case CimType.UInt32: return typeof(uint[]);
          case CimType.UInt64: return typeof(ulong[]);
          case CimType.UInt8: return typeof(byte[]);
          default: return typeof(object[]);
        }
      }
      else
      {
        switch (property.Type)
        {
          case CimType.Boolean: return typeof(bool);
          case CimType.Char16: return typeof(char);
          case CimType.DateTime: return typeof(DateTime);
          case CimType.Real32: return typeof(float);
          case CimType.Real64: return typeof(double);
          case CimType.SInt16: return typeof(short);
          case CimType.SInt32: return typeof(int);
          case CimType.SInt64: return typeof(long);
          case CimType.SInt8: return typeof(sbyte);
          case CimType.String: return typeof(string);
          case CimType.UInt16: return typeof(ushort);
          case CimType.UInt32: return typeof(uint);
          case CimType.UInt64: return typeof(ulong);
          case CimType.UInt8: return typeof(byte);
          default: return typeof(object);
        }
      }
    }
  }
}