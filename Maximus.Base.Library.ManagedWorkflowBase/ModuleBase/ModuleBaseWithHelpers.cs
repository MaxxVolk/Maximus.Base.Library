// .Net Framework
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;
using Microsoft.EnterpriseManagement.Modules.DataItems.Discovery;
using Microsoft.EnterpriseManagement.Mom.Modules.DataItems;

namespace Maximus.Library.ManagedModuleBase
{
  public enum ModuleErrorCriticality { Continue, ThrowAndContinue, Stop }

  /// <summary>
  /// Extend base SCOM managed module base class with static service functions.
  /// </summary>
  /// <typeparam name="TOutputDataType">Output data type for the an action. <typeparamref name="PropertyBagDataItem"/> is the most
  /// common output type for a monitoring probe action. Use <typeparamref name="DiscoveryDataItem"/> for a discovery
  /// probe action.</typeparam>
  public abstract class ModuleBaseWithHelpers<TOutputDataType> : ModuleBase<TOutputDataType> where TOutputDataType : DataItemBase
  {
    // cached values
    private Version internal_ModuleVersion;
    private string internal_ModuleName;

    public ModuleBaseWithHelpers(ModuleHost<TOutputDataType> moduleHost) : base(moduleHost)
    {
      if (moduleHost == null)
        throw new ArgumentNullException(nameof(moduleHost));
    }

    public static void LoadConfigurationElement(XmlDocument cfgDoc, string paramName, out Guid paramValue, string defaultValue = "{00000000-0000-0000-0000-000000000000}", bool Compulsory = true)
    {
      LoadConfigurationElement(cfgDoc, paramName, out string varOut, defaultValue, Compulsory);
      paramValue = new Guid(varOut);
    }

    public static void LoadConfigurationElement(XmlDocument cfgDoc, string paramName, out string paramValue, string defaultValue = "", bool Compulsory = true)
    {
      // GetConfigurationElement takes care about throwing exception if compulsory element is missing
      string serializedString = GetConfigurationElement(cfgDoc, paramName, Compulsory);
      if (serializedString != null)
        paramValue = serializedString;
      else
        paramValue = defaultValue;
      return;
    }

    public static void LoadConfigurationElement(XmlDocument cfgDoc, string paramName, out bool paramValue, bool defaultValue = false, bool Compulsory = true)
    {
      // GetConfigurationElement takes care about throwing exception if compulsory element is missing
      string serializedBoolean = GetConfigurationElement(cfgDoc, paramName, Compulsory);
      if (!string.IsNullOrEmpty(serializedBoolean))
        paramValue = ConvertFromSCOMBoolean(serializedBoolean);
      else
        paramValue = defaultValue;
      return;
    }

    public static void LoadConfigurationElement(XmlDocument cfgDoc, string paramName, out int paramValue, int defaultValue = 0, bool Compulsory = true)
    {
      // GetConfigurationElement takes care about throwing exception if compulsory element is missing
      string serializedInteger = GetConfigurationElement(cfgDoc, paramName, Compulsory);
      if (!string.IsNullOrEmpty(serializedInteger))
        paramValue = int.Parse(serializedInteger);
      else
        paramValue = defaultValue;
      return;
    }

    public static void LoadConfigurationElement(XmlDocument cfgDoc, string paramName, out double paramValue, double defaultValue = 0, bool Compulsory = true)
    {
      // GetConfigurationElement takes care about throwing exception if compulsory element is missing
      string serializedDouble = GetConfigurationElement(cfgDoc, paramName, Compulsory);
      if (!string.IsNullOrEmpty(serializedDouble))
        paramValue = double.Parse(serializedDouble);
      else
        paramValue = defaultValue;
      return;
    }

    protected static string GetConfigurationElement(XmlDocument cfgDoc, string paramName, bool Compulsory = true)
    {
      XmlNodeList nodes = null;
      nodes = cfgDoc.GetElementsByTagName(paramName);
      if (nodes != null)
      {
        if (nodes.Count == 1)
        {
          if (!string.IsNullOrEmpty(nodes[0].InnerText))
            return nodes[0].InnerText;
          else
            return null;
        }
        if (nodes.Count >= 2)
          throw new ModuleBaseException("Ambiguous configuration element name: " + paramName + ". Number of elements: " + nodes.Count.ToString() + ".");
        if (nodes.Count == 0)
        {
          if (Compulsory)
            throw new ModuleBaseException("Missing compulsory configuration element: " + paramName + ".");
          else
            return null;
        }
      }
      throw new ModuleBaseException("Failure in .Net XML framework.");
    }

    public static bool ConvertFromSCOMBoolean(string BoolString)
    {
      bool _Result;
      _Result = false;
      if (!string.IsNullOrEmpty(BoolString))
      { _Result = string.Equals(BoolString, "true", StringComparison.OrdinalIgnoreCase) | string.Equals(BoolString, "1", StringComparison.OrdinalIgnoreCase); }
      return _Result;
    }

    public static string ConvertToSCOMBoolean(bool BoolInput)
    {
      return BoolInput.ToString().ToLowerInvariant();
    }

    public static Property NewProperty(string name, string value) => new Property(name, value);
    public static Property NewProperty(Guid id, string value) => new Property(id.ToString("B"), value);
    public static Property NewProperty(Guid id, object value) => new Property(id.ToString("B"), value.ToString());
    public static Property NewProperty(Guid id, DateTime value) => new Property(id.ToString("B"), value.ToString("yyyy-MM-dd HH:mm:ss"));
    public static Property NewProperty(Guid id, bool value) => new Property(id.ToString("B"), value ? "True" : "False");

    public static PropertyBagDataItem CreatePropertyBag(Dictionary<string, object> bagItem)
    {
      Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>
      {
        { "", bagItem }
      };
      return new PropertyBagDataItem(null, dictionary);
    }

    public static PropertyBagDataItem CreatePropertyBag(params object[] pairs)
    {
      if (pairs == null)
        return null;
      if (pairs.Length % 2 != 0)
        throw new ArgumentOutOfRangeException(nameof(pairs), "Argument count must be even.");

      Dictionary<string, object> bagItem = new Dictionary<string, object>();
      for (int j = 0; j < pairs.Length / 2; j++)
        bagItem.Add(pairs[j].ToString(), pairs[j + 1]);

      return new PropertyBagDataItem(null, new Dictionary<string, Dictionary<string, object>>
      {
        { "", bagItem }
      });
    }

    public Version ModuleVersion
    {
      get
      {
        if (internal_ModuleVersion != null)
          return internal_ModuleVersion;
        try
        {
          internal_ModuleVersion = Assembly.GetAssembly(GetType()).GetName().Version;
          return internal_ModuleVersion;
        }
        catch (Exception e)
        {
          try
          {
            string msg = "Failed to get ModuleVersion proverty via assembly reflection. Returning default value.";
            ModuleErrorSignalReceiver(ModuleErrorSeverity.Information, ModuleErrorCriticality.Continue, new ModuleBaseException(msg, e), msg);
          }
          catch { } // ignore
          return new Version(0, 0, 0, 0);
        }
      }
    }

    public string ModuleName
    {
      get
      {
        if (!string.IsNullOrEmpty(internal_ModuleName))
          return internal_ModuleName;
        try
        {
          internal_ModuleName = GetType().FullName;
          return internal_ModuleName;
        }
        catch (Exception e)
        { 
          try
          {
            string msg = "Failed to get ModuleName proverty via assembly reflection. Returning default value.";
            ModuleErrorSignalReceiver(ModuleErrorSeverity.Information, ModuleErrorCriticality.Continue, new ModuleBaseException(msg, e), msg);
          }
          catch { } // ignore
          return "ModuleBaseWithServiceFunctions";
        }
      }
    }

    protected abstract void ModuleErrorSignalReceiver(ModuleErrorSeverity severity, ModuleErrorCriticality criticality, Exception e, string message, params object[] extaInfo);
  }
}