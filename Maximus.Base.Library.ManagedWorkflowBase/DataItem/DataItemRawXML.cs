// .Net Framework
using System.Xml;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;

namespace Maximus.Library.DataItemCollection
{
  [MonitoringDataType]
  public class RawXMLDataItem : DataItemBase
  {
    private readonly string dataItemTypeName = "Maximus.Library.RawXMLData";
    private readonly string wholeXML;

    public RawXMLDataItem(XmlReader reader) : base(reader)
    {
      wholeXML = reader.ReadOuterXml();
    }

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

    public override string DataItemTypeName => dataItemTypeName;

    protected override void GenerateItemXml(XmlWriter writer) => writer.WriteRaw(wholeXML);
  }

}