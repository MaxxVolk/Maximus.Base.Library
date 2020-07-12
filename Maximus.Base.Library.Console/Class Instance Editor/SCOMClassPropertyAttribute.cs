using System;

namespace Maximus.Library.SCOM.Editors
{
  [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  sealed public class SCOMClassPropertyAttribute : Attribute
  {

    // This is a positional argument
    public SCOMClassPropertyAttribute(string propertyGuid) => PropertyId = Guid.Parse(propertyGuid);

    public Guid PropertyId { get; }
  }
}