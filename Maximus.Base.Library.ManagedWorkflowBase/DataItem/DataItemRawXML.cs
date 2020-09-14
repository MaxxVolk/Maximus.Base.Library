// .Net Framework
using System.Xml;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;

namespace Maximus.Library.DataItemCollection
{
  /// <summary>
  /// Class to create a Data Item from raw XML.
  /// </summary>
  [MonitoringDataType]
  public class RawXMLDataItem : DataItemBase
  {
    private readonly string dataItemTypeName = "Maximus.Library.RawXMLData";
    private readonly string wholeXML;

    /// <summary>
    /// Creates Data Item from XML in <paramref name="reader"/>
    /// </summary>
    /// <param name="reader">Reader object for the source XML</param>
    public RawXMLDataItem(XmlReader reader) : base(reader)
    {
      wholeXML = reader.ReadOuterXml();
    }

    /// <summary>
    /// Creates Data Item from XML in <paramref name="reader"/> and set Data Item type to <paramref name="CustomDataItemTypeName"/>.
    /// </summary>
    /// <param name="CustomDataItemTypeName"></param>
    /// <param name="reader">Reader object for the source XML</param>
    public RawXMLDataItem(string CustomDataItemTypeName, XmlReader reader) : base(reader)
    {
      string currentElement;
      do
      {
        currentElement = reader.ReadOuterXml();
        wholeXML += currentElement;
      } while (!string.IsNullOrEmpty(currentElement));
      
      dataItemTypeName = CustomDataItemTypeName;
    }

    /// <summary>
    /// Current Data Item type.
    /// </summary>
    public override string DataItemTypeName => dataItemTypeName;

    /// <summary>
    /// Writes resultant XML of the current Data Item using <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">Where to write item's XML.</param>
    protected override void GenerateItemXml(XmlWriter writer) => writer.WriteRaw(wholeXML);
  }

}