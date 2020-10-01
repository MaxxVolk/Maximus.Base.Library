// .Net Framework
using System.Xml;
using System.Xml.Serialization;

namespace Maximus.Library.DataItemCollection
{
  /// <summary>
  /// This is a copy of <code>Maximus.Library.DataItemCollection.SerializationDataContainerDataItemBase<T></code> class from 
  /// Maximus.Base.Library.ManagedWorkflowBase library, but without reference to Microsoft.EnterpriseManagement.HealthService
  /// which is not available in console assemblies.
  /// </summary>
  /// <typeparam name="T">Data class inherited from SerializationData</typeparam>
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

    protected abstract string GetDataItemTypeName();
  }
}