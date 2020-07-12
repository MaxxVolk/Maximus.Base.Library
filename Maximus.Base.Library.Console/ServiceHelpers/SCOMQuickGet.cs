using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
//using Microsoft.EnterpriseManagement.InternalSdkOnly;
using System.Security.Cryptography;

namespace Maximus.Library.Helpers
{
  public static class SCOMQuickGet
  {
    public static T GetObjectInstance<T>(this IMonitoringObjectsManagement monitoringObjects, Guid managementPackClassId, IDictionary<Guid, object> keyProperties, ObjectQueryOptions queryOptions) where T: EnterpriseManagementObject
    {
      StringBuilder hashString = new StringBuilder();
      hashString.Append("TypeId=");
      hashString.Append(managementPackClassId.ToString("B").ToUpperInvariant());
      if (keyProperties != null)
        foreach (KeyValuePair<Guid, object> keyProperty in keyProperties)
        {
          hashString.Append(",");
          hashString.Append(keyProperty.Key.ToString("B").ToUpperInvariant());
          hashString.Append("=");
          hashString.Append(keyProperty.Value.ToString());
        }
      return monitoringObjects.GetObject<T>(GetGuidFromString(hashString.ToString()), queryOptions);
    }

    public static Guid GetObjectId(Guid managementPackClassId, IDictionary<Guid, object> keyProperties)
    {
      StringBuilder hashString = new StringBuilder();
      hashString.Append("TypeId=");
      hashString.Append(managementPackClassId.ToString("B").ToUpperInvariant());
      if (keyProperties != null)
        foreach (KeyValuePair<Guid, object> keyProperty in keyProperties)
        {
          hashString.Append(",");
          hashString.Append(keyProperty.Key.ToString("B").ToUpperInvariant());
          hashString.Append("=");
          hashString.Append(keyProperty.Value.ToString());
        }
      return GetGuidFromString(hashString.ToString());
    }

    public static T GetWindowsComputerInstance<T>(this IMonitoringObjectsManagement monitoringObjects, string computerName, ObjectQueryOptions queryOptions) where T : EnterpriseManagementObject
    {
      return monitoringObjects.GetObjectInstance<T>(new Guid("ea99500d-8d52-fc52-b5a5-10dcd1e9d2bd"), new Dictionary<Guid, object>() { { new Guid("5c324096-d928-76db-e9e7-e629dcc261b1"), computerName.ToUpperInvariant() } }, queryOptions);
    }

    public static T GetHealthServiceInstance<T>(this IMonitoringObjectsManagement monitoringObjects, string computerName, ObjectQueryOptions queryOptions) where T : EnterpriseManagementObject
    {
      return monitoringObjects.GetObjectInstance<T>(new Guid("ab4c891f-3359-3fb6-0704-075fbfe36710"), new Dictionary<Guid, object>() { { new Guid("5c324096-d928-76db-e9e7-e629dcc261b1"), computerName.ToUpperInvariant() } }, queryOptions);
    }

    public static ManagementPackClass GetClass(this IEntityTypeManagement entityTypeManagement, string mpName, string mpToken, string className) => entityTypeManagement.GetClass(GetManagementPackElementId(mpName, mpToken, className));

    public static Guid GetManagementPackElementId(string mpName, string mpToken, string elementName) => GetGuidFromString(GetManagementPackElementReference(mpName, mpToken, elementName));

    public static string GetManagementPackElementReference(string mpName, string mpToken, string elementName)
    {
      if (string.IsNullOrEmpty(mpName))
        return $"ObjectId={elementName}";
      if (mpName == elementName)
      {
        if (string.IsNullOrEmpty(mpToken))
          return $"MPName={mpName}";
        else
          return $"MPName={mpName},KeyToken={mpToken}";
      }
      if (string.IsNullOrEmpty(mpToken))
        return $"MPName={mpName},ObjectId={elementName}";
      return $"MPName={mpName},KeyToken={mpToken},ObjectId={elementName}";
    }

    /// <summary>
    /// (c) Microsoft.
    /// </summary>
    /// <param name="textToHash"></param>
    /// <returns></returns>
    public static Guid GetGuidFromString(string textToHash)
    {
      byte[] hash;
      using (SHA1 shA1 = new SHA1CryptoServiceProvider())
      {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        byte[] buffer = textToHash != null ? unicodeEncoding.GetBytes(textToHash.ToString()) : unicodeEncoding.GetBytes("<null>");
        hash = shA1.ComputeHash(buffer);
      }
      return new Guid(hash[3] << 24 | hash[2] << 16 | hash[1] << 8 | hash[0], (short)(hash[5] << 8 | hash[4]), (short)(hash[7] << 8 | hash[6]), hash[8], hash[9], hash[10], hash[11], hash[12], hash[13], hash[14], hash[15]);
    }
  }
}