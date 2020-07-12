using System;
using System.Collections.Generic;
using System.Data;
using System.Management;

namespace Maximus.Library.Helpers
{
  public static class WMIHelper
  {
    public static ManagementScope LocalManagementScope => new ManagementScope(ManagementPath.DefaultPath);
    public static ManagementBaseObject InvokeWMIMethod(this ManagementObject managementObject, string methodName, Dictionary<string, object> parameters, InvokeMethodOptions methodOptions = null)
    {
      ManagementBaseObject inParams = managementObject.GetMethodParameters(methodName);
      foreach(KeyValuePair<string, object> param in parameters)
        inParams[param.Key] = param.Value;
      return managementObject.InvokeMethod(methodName, inParams, methodOptions);
    }

    public static DataTable GetWMIQuery(string computerName, string WQLquery, string WMInamespace = @"\root\cimv2")
    {
      ManagementScope scope = new ManagementScope("\\\\" + ComputerHelper.LocalizeName(computerName, ".") + WMInamespace);
      scope.Connect();
      return GetWMIQuery(scope, WQLquery, WMInamespace);
    }

    public static DataTable GetWMIQuery(string WQLquery, string WMInamespace = @"\root\cimv2")
    {
      ManagementScope scope = new ManagementScope(WMInamespace);
      scope.Connect();
      return GetWMIQuery(scope, WQLquery, WMInamespace);
    }

    public static DataTable GetWMIQuery(ManagementScope scope, string WQLquery, string WMInamespace = @"\root\cimv2")
    {
      if (!scope.IsConnected)
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