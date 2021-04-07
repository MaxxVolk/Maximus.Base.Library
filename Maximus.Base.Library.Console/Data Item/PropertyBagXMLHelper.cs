using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Maximus.Library.DataItemCollection
{
  /// <summary>
  /// Use this class to access PropertyBagDataItem properties in console extensions library. You must NOT reference any SCOM Agent aka Health Service
  /// library, including data item implementation ones, therefore, it's not possible to use native PropertyBagDataItem implementation.
  /// </summary>
  public class PropertyBagXMLHelper
  {
    protected XmlDocument xmlDocument;

    /// <summary>
    /// Create new XML parser for serialized PropertyBagDataItem.
    /// </summary>
    /// <param name="serializedPropertyBag">XML serialized instance of PropertyBagDataItem</param>
    public PropertyBagXMLHelper(string serializedPropertyBag)
    {
      xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(serializedPropertyBag);
      Property = new Dictionary<string, object>();
      foreach (XmlNode propertyNode in xmlDocument["DataItem"].ChildNodes)
        if (propertyNode.Name == "Property")
        {
          object data = null;
          // https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualbasic.varianttype?view=netcore-3.1
          switch (int.Parse(propertyNode.Attributes["VariantType"].Value))
          {
            // case 8192: == Array, not supported
            case 11: data = bool.Parse(propertyNode.InnerText); break;
            case 8: data = propertyNode.InnerText; break;
            case 17: data = byte.Parse(propertyNode.InnerText); break;
            case 18: data = propertyNode.InnerText[0]; break;
            // case 6: currency, not supported
            // case 13: DataObject, not supported
            case 7: data = DateTime.ParseExact(propertyNode.InnerText, "O", null); break;
            case 14: data = decimal.Parse(propertyNode.InnerText); break;
            case 5: data = double.Parse(propertyNode.InnerText); break;
            case 1:
            case 0: data = null; break;
            // case 10: Exception, not supported
            // case 9: Object, not supported
            case 3: data = int.Parse(propertyNode.InnerText); break;
            case 20: data = long.Parse(propertyNode.InnerText); break;
            case 2: data = short.Parse(propertyNode.InnerText); break;
            case 4: data = float.Parse(propertyNode.InnerText); break;
            // case 36: UserDefinedType, not supported
            // case 12: Variant, not supported
          }
          Property.Add(propertyNode.Attributes["Name"].Value, data);
        }
    }

    public string ItemDataType => xmlDocument["DataItem"].Attributes["type"].Value;

    public DateTime Timestamp => DateTime.ParseExact(xmlDocument["DataItem"].Attributes["time"].Value, "O", null);

    public Guid SourceHealthServiceId => new Guid(xmlDocument["DataItem"].Attributes["sourceHealthServiceId"].Value);

    /// <summary>
    /// Direct access to bag's properties.
    /// </summary>
    public Dictionary<string, object> Property { get; protected set; }

    /// <summary>
    /// Get property value by name from the source property bag.
    /// </summary>
    /// <typeparam name="T">Cast result as type.</typeparam>
    /// <param name="name">Property name</param>
    /// <returns>Value of property.</returns>
    /// <exception cref="KeyNotFoundException" />
    /// <exception cref="InvalidCastException" />
    public T GetPropertyAs<T>(string name)
    {
      if (Property.ContainsKey(name))
        return (T)Property[name];
      else
        return default(T);
    }
  }
}
