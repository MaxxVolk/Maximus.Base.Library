using Microsoft.EnterpriseManagement.Mom.Modules.DataItems;

using System.Collections.Generic;
using System.Reflection;

namespace Maximus.Library.DataItemCollection
{
  /// <summary>
  /// Inherit from this object to create a resultant data class for workflows returning PropertyBag data item.
  /// All public properties will be included into property bag upon call of GetPropertyBagDataItem() method.
  /// </summary>
  public class PropertyBagObject
  {
    /// <summary>
    /// Call this method to return a PropertyBag data item with all object public property.
    /// </summary>
    /// <returns>PropertyBag data item</returns>
    public PropertyBagDataItem GetPropertyBagDataItem()
    {
      Dictionary<string, object> bagItem = new Dictionary<string, object>();
      foreach (PropertyInfo myProperty in GetType().GetProperties())
      {
        bagItem.Add(myProperty.Name, myProperty.GetValue(this));
      }
      Dictionary<string, Dictionary<string, object>> collections = new Dictionary<string, Dictionary<string, object>> { { "", bagItem } };
      return new PropertyBagDataItem(null, collections);
    }
  }
}
