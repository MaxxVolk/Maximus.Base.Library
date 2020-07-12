// .Net Framework
using System.Xml;
using System.Xml.Serialization;

namespace Maximus.Library.DataItemCollection
{
  public abstract class SerializationDataContainerDataItemBase<T>  where T : SerializationData
  {
    public T Data { get; }

    public SerializationDataContainerDataItemBase(T data)
    {
      Data = data;
    }

    public SerializationDataContainerDataItemBase(XmlReader reader)
    {
      XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
      Data = (T)xmlFormat.Deserialize(reader);
    }

    protected void GenerateItemXml(XmlWriter writer)
    {
      XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
      xmlFormat.Serialize(writer, Data);
    }
  }
}