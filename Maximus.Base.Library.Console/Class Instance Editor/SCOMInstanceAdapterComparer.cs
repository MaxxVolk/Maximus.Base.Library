using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Maximus.Library.SCOM.Editors
{
  public class SCOMInstanceAdapterComparer<A> : IComparer<A> where A : SCOMClassInstanceAdapter
  {
    private PropertyDescriptor sortingProperty;
    private readonly ListSortDirection sortingdirection;
    public SCOMInstanceAdapterComparer(PropertyDescriptor property, ListSortDirection direction)
    {
      sortingProperty = property;
      sortingdirection = direction;
    }

    public int Compare(A x, A y)
    {
      object xValue = sortingProperty.GetValue(x);
      object yValue = sortingProperty.GetValue(y);
      if (xValue is null && yValue is null)
        return 0;
      if (sortingProperty.Name == nameof(SCOMClassInstanceAdapter.BindableActionPoint) || sortingProperty.Name == nameof(SCOMClassInstanceAdapter.ActionPoint))
      {
        // special handling
        string xDisplayName = null;
        string yDisplayName = null;
        xDisplayName = x?.ActionPoint?.DisplayName;
        yDisplayName = y?.ActionPoint?.DisplayName;
        if (sortingdirection == ListSortDirection.Ascending)
          return xDisplayName is null ? -1 : xDisplayName.CompareTo(yDisplayName);
        else
          return yDisplayName is null ? 0 : yDisplayName.CompareTo(xDisplayName);
      }
      else
      {
        // generic handler
        if (sortingdirection == ListSortDirection.Ascending)
          return xValue is null ? -1 : ((IComparable)xValue).CompareTo(yValue);
        else
          return yValue is null ? 1 : ((IComparable)yValue).CompareTo(xValue);
      }
    }
  }
}