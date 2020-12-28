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
  /// <summary>
  /// Describes criticality of module's error.
  /// </summary>
  public enum ModuleErrorCriticality { 
    /// <summary>
    /// Module continue to run.
    /// </summary>
    Continue, 
    /// <summary>
    /// Throw an exception, but continue to run.
    /// </summary>
    ThrowAndContinue, 
    /// <summary>
    /// Module stops running.
    /// </summary>
    Stop 
  }

  /// <summary>
  /// Extend base SCOM managed module base class with static service functions.
  /// </summary>
  /// <typeparam name="TOutputDataType">Output data type for the an action. <seealso cref="PropertyBagDataItem"/> is the most
  /// common output type for a monitoring probe action. Use <seealso cref="DiscoveryDataItem"/> for a discovery
  /// probe action.</typeparam>
  public abstract class ModuleBaseWithHelpers<TOutputDataType> : ModuleBase<TOutputDataType> where TOutputDataType : DataItemBase
  {
    // cached values
    private Version internal_ModuleVersion;
    private string internal_ModuleName;

    /// <summary>
    /// Shall never create class instances directly.
    /// </summary>
    /// <param name="moduleHost"></param>
    public ModuleBaseWithHelpers(ModuleHost<TOutputDataType> moduleHost) : base(moduleHost)
    {
      if (moduleHost == null)
        throw new ArgumentNullException(nameof(moduleHost));
    }

    /// <summary>
    /// Read module configuration parameter of <seealso cref="Guid"/> type.
    /// </summary>
    /// <param name="cfgDoc">Module's XML configuration.</param>
    /// <param name="paramName">Parameter name</param>
    /// <param name="paramValue">Output variable for parameter value</param>
    /// <param name="defaultValue">Default value if parameter is not in the XML configuration</param>
    /// <param name="Compulsory"><c>true</c> if configuration element is compulsory. If compulsory element is not found, an <seealso cref="ModuleBaseException"/> will be thrown</param>
    public static void LoadConfigurationElement(XmlDocument cfgDoc, string paramName, out Guid paramValue, string defaultValue = "{00000000-0000-0000-0000-000000000000}", bool Compulsory = true)
    {
      LoadConfigurationElement(cfgDoc, paramName, out string varOut, defaultValue, Compulsory);
      paramValue = new Guid(varOut);
    }

    /// <summary>
    /// Read module configuration parameter of <seealso cref="string"/> type.
    /// </summary>
    /// <param name="cfgDoc"></param>
    /// <param name="paramName"></param>
    /// <param name="paramValue"></param>
    /// <param name="defaultValue"></param>
    /// <param name="Compulsory"><c>true</c> if configuration element is compulsory. If compulsory element is not found, an <seealso cref="ModuleBaseException"/> will be thrown</param>
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

    /// <summary>
    /// Read module configuration parameter of <seealso cref="bool"/> type.
    /// </summary>
    /// <param name="cfgDoc"></param>
    /// <param name="paramName"></param>
    /// <param name="paramValue"></param>
    /// <param name="defaultValue"></param>
    /// <param name="Compulsory"><c>true</c> if configuration element is compulsory. If compulsory element is not found, an <seealso cref="ModuleBaseException"/> will be thrown</param>
    public static void LoadConfigurationElement(XmlDocument cfgDoc, string paramName, out bool paramValue, bool defaultValue = false, bool Compulsory = true)
    {
      // GetConfigurationElement takes care about throwing exception if compulsory element is missing
      string serializedBoolean = GetConfigurationElement(cfgDoc, paramName, Compulsory);
      if (!string.IsNullOrEmpty(serializedBoolean))
        paramValue = ConvertSCOMBoolean(serializedBoolean);
      else
        paramValue = defaultValue;
      return;
    }

    /// <summary>
    /// Read module configuration parameter of <seealso cref="int"/> type.
    /// </summary>
    /// <param name="cfgDoc"></param>
    /// <param name="paramName"></param>
    /// <param name="paramValue"></param>
    /// <param name="defaultValue"></param>
    /// <param name="Compulsory"><c>true</c> if configuration element is compulsory. If compulsory element is not found, an <seealso cref="ModuleBaseException"/> will be thrown</param>
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

    /// <summary>
    /// Read module configuration parameter of <seealso cref="double"/> type.
    /// </summary>
    /// <param name="cfgDoc"></param>
    /// <param name="paramName"></param>
    /// <param name="paramValue"></param>
    /// <param name="defaultValue"></param>
    /// <param name="Compulsory"><c>true</c> if configuration element is compulsory. If compulsory element is not found, an <seealso cref="ModuleBaseException"/> will be thrown</param>
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

    /// <summary>
    /// Loads raw text content of an XML element.
    /// </summary>
    /// <param name="cfgDoc">Module's configuration XML document</param>
    /// <param name="paramName">Name of parameter to load</param>
    /// <param name="Compulsory"><c>true</c> if configuration element is compulsory. If compulsory element is not found, an <seealso cref="ModuleBaseException"/> will be thrown</param>
    /// <returns></returns>
    protected static string GetConfigurationElement(XmlDocument cfgDoc, string paramName, bool Compulsory = true)
    {
      XmlNodeList nodes = cfgDoc.GetElementsByTagName(paramName);
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

    /// <summary>
    /// Converts Management Pack style serialized <c>boolean</c> to C# <c>boolean</c>.
    /// </summary>
    /// <param name="BoolString"></param>
    /// <returns>Deserialized value.</returns>
    public static bool ConvertSCOMBoolean(string BoolString)
    {
      if (!string.IsNullOrEmpty(BoolString))
        return string.Equals(BoolString, "true", StringComparison.OrdinalIgnoreCase) | string.Equals(BoolString, "1", StringComparison.OrdinalIgnoreCase);
      else
        throw new ArgumentNullException(nameof(BoolString));
    }

    /// <summary>
    /// Serialize <paramref name="BoolInput"/> to Management Pack style serialized <c>boolean</c>.
    /// </summary>
    /// <param name="BoolInput"></param>
    /// <returns></returns>
    public static string ConvertToSCOMBoolean(bool BoolInput)
    {
      return BoolInput.ToString().ToLowerInvariant();
    }

    /// <summary>
    /// Creates a new <see cref="Property"/> object from property name and serialized value.
    /// </summary>
    /// <param name="name">Property name</param>
    /// <param name="value">Property serialized value</param>
    /// <returns>New <see cref="Property"/> object</returns>
    public static Property NewProperty(string name, string value) => new Property(name, value);

    /// <summary>
    /// Creates a new <see cref="Property"/> object from property id and serialized value.
    /// </summary>
    /// <param name="id">Property name</param>
    /// <param name="value">Property serialized value</param>
    /// <returns>New <see cref="Property"/> object</returns>
    public static Property NewProperty(Guid id, string value) => new Property(id.ToString("B"), value);

    /// <summary>
    /// Creates a new <see cref="Property"/> object from property id and value. <b>WARNING!</b> Default object serialization is used.
    /// </summary>
    /// <param name="id">Property name</param>
    /// <param name="value">Property serialized value</param>
    /// <returns>New <see cref="Property"/> object</returns>
    public static Property NewProperty(Guid id, object value) => new Property(id.ToString("B"), value.ToString());

    /// <summary>
    /// Creates a new <see cref="Property"/> object from property id and date-time value using ODBC serialization.
    /// </summary>
    /// <param name="id">Property name</param>
    /// <param name="value">Property serialized value</param>
    /// <returns>New <see cref="Property"/> object</returns>
    public static Property NewProperty(Guid id, DateTime value) => new Property(id.ToString("B"), value.ToString("yyyy-MM-dd HH:mm:ss"));

    /// <summary>
    /// Creates a new <see cref="Property"/> object from property id and boolean value using Management Pack style serialization.
    /// </summary>
    /// <param name="id">Property name</param>
    /// <param name="value">Property serialized value</param>
    /// <returns>New <see cref="Property"/> object</returns>
    public static Property NewProperty(Guid id, bool value) => new Property(id.ToString("B"), value ? "True" : "False");

    /// <summary>
    /// Creates a <c>PropertyBagDataItem</c> object.
    /// </summary>
    /// <param name="bagItem">Inbound key-value dictionary.</param>
    /// <returns>New <c>PropertyBagDataItem</c> object.</returns>
    public static PropertyBagDataItem CreatePropertyBag(Dictionary<string, object> bagItem)
    {
      Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>
      {
        { "", bagItem }
      };
      return new PropertyBagDataItem(null, dictionary);
    }

    /// <summary>
    /// Created a new <c>PropertyBagDataItem</c> object from a Key-Value list. Odd array elements must contain Key, and event elements must contain Value.
    /// For example: <code>CreatePropertyBag("Key1", "Value1", "Key2", 100);</code>
    /// </summary>
    /// <param name="pairs">Key-Value list.</param>
    /// <returns>New <c>PropertyBagDataItem</c> object.</returns>
    public static PropertyBagDataItem CreatePropertyBag(params object[] pairs)
    {
      if (pairs == null)
        return null;
      if (pairs.Length % 2 != 0)
        throw new ArgumentOutOfRangeException(nameof(pairs), "Argument count must be even.");

      Dictionary<string, object> bagItem = new Dictionary<string, object>();
      for (int j = 0; j < pairs.Length / 2; j++)
        bagItem.Add(pairs[j * 2].ToString(), pairs[j * 2 + 1]);

      return new PropertyBagDataItem(null, new Dictionary<string, Dictionary<string, object>>
      {
        { "", bagItem }
      });
    }

    /// <summary>
    /// Module version, in which this class is defined. Used for logging.
    /// </summary>
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
            string msg = "Failed to get ModuleVersion property via assembly reflection. Returning default value.";
            ModuleErrorSignalReceiver(ModuleErrorSeverity.Information, ModuleErrorCriticality.Continue, new ModuleBaseException(msg, e), msg);
          }
          catch { } // ignore
          return new Version(0, 0, 0, 0);
        }
      }
    }

    /// <summary>
    /// Module name, in which this class is defined. Used for logging.
    /// </summary>
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
            string msg = "Failed to get ModuleName property via assembly reflection. Returning default value.";
            ModuleErrorSignalReceiver(ModuleErrorSeverity.Information, ModuleErrorCriticality.Continue, new ModuleBaseException(msg, e), msg);
          }
          catch { } // ignore
          return "ModuleBaseWithServiceFunctions";
        }
      }
    }

    /// <summary>
    /// Module internal methods call this class for any module-related errors or exceptions. 
    /// Override this method to receive exception notifications.
    /// </summary>
    /// <param name="severity"></param>
    /// <param name="criticality"></param>
    /// <param name="e"></param>
    /// <param name="message"></param>
    /// <param name="extaInfo"></param>
    protected virtual void ModuleErrorSignalReceiver(ModuleErrorSeverity severity, ModuleErrorCriticality criticality, Exception e, string message, params object[] extaInfo)
    {
      // do nothing by default.
    }
  }
}