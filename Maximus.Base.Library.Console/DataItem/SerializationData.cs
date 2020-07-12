// .Net Framework
using System;
using System.Xml.Serialization;

namespace Maximus.Library.DataItemCollection
{
  public abstract class SerializationData
  {
    public SerializationData()
    {
      object[] xmlArrtibute = GetType().GetCustomAttributes(typeof(XmlRootAttribute), true);
      if (xmlArrtibute == null || xmlArrtibute.Length != 1)
        throw new InvalidCastException("Types inherited from SerializationData must have XmlRoot attribute.");
    }
  }
}