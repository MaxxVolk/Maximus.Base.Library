// .Net Framework
using System.Xml;
using System.Xml.Serialization;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;

namespace Maximus.Library.DataItemCollection
{
  public abstract class SerializationDataContainerDataItemBase<T> : DataItemBase where T : SerializationData
  {
    public T Data { get; }

    public SerializationDataContainerDataItemBase(T data)
    {
      Data = data;
    }

    public SerializationDataContainerDataItemBase(XmlReader reader) : base(reader)
    {
      XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
      Data = (T)xmlFormat.Deserialize(reader);
    }

    protected abstract string GetDataItemTypeName();

    public override string DataItemTypeName
    {
      get
      {
        return GetDataItemTypeName();
      }
    }

    protected override void GenerateItemXml(XmlWriter writer)
    {
      XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
      xmlFormat.Serialize(writer, Data);
    }
  }
}