using System.Collections.Generic;
using System.ComponentModel;

namespace Maximus.Library.SCOM.Editors
{
  public class SCOMInstanceBindingList<T> : BindingList<T> where T : SCOMClassInstanceAdapter
  {
    #region Sorting Implementation
    // Sorting variable
    protected bool isSorted = false;
    protected PropertyDescriptor sortingProperty;
    protected ListSortDirection sortingDirection;

    // Sorting properties
    protected override bool SupportsSortingCore => true;

    protected override bool IsSortedCore => isSorted;

    protected override PropertyDescriptor SortPropertyCore => sortingProperty;

    protected override ListSortDirection SortDirectionCore => sortingDirection;

    // Sorting Methods
    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
      sortingDirection = direction;
      sortingProperty = prop;
      if (!(Items is null))
      {
        (Items as List<T>).Sort(new SCOMInstanceAdapterComparer<T>(prop, direction));
        isSorted = true;
      }
       else
      {
        isSorted = false;
      }
      OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    protected override void RemoveSortCore()
    {
      isSorted = false;
      sortingProperty = null;
    }
    #endregion

    #region Searching Implementation
    protected override bool SupportsSearchingCore => true;
    protected override int FindCore(PropertyDescriptor prop, object key)
    {
      SCOMInstanceAdapterPropertyComparer<T> searchComparer = new SCOMInstanceAdapterPropertyComparer<T>(prop);
      if (Items != null)
        return (Items as List<T>).FindIndex(x => searchComparer.Compare(x, key) == 0);
      return -1;
    }
    #endregion
  }
}