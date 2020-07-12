using System;
using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Monitoring;

namespace Maximus.Library.SCOM.Editors
{
  public class BindableMonitoringObject : IEquatable<BindableMonitoringObject>
  {
    public BindableMonitoringObject(MonitoringObject monitoringObject) => MonitoringObject = monitoringObject;

    public BindableMonitoringObject Self { get => this; }

    public MonitoringObject MonitoringObject { get; set; }

    public string Name { get => (MonitoringObject is null) ? "<unassigned>" : MonitoringObject.DisplayName; }

    public override bool Equals(object obj)
    {
      return Equals(obj as BindableMonitoringObject);
    }

    public bool Equals(BindableMonitoringObject other)
    {
      return (other != null) && (this == other);
    }

    public override int GetHashCode()
    {
      return -196810745 + EqualityComparer<MonitoringObject>.Default.GetHashCode(MonitoringObject);
    }

    public override string ToString() => Name;

    private static bool InternalCompare(BindableMonitoringObject m1, BindableMonitoringObject m2)
    {
      // this code is not 100% correct, but it works since SCOMInstanceAdapter returns null for null MonitoringObject value
      // compare nulls
      if (m1 is null && m2 is null)
        return true;
      if (!(m1 is null))
        if (m1.MonitoringObject is null && m2 is null)
          return true;
      if (!(m2 is null))
        if (m2.MonitoringObject is null && m1 is null)
          return true;

      if (m1 is null || m2 is null)
        return false;
      if (m1.MonitoringObject is null && m2.MonitoringObject is null)
        return true;
      if (m1.MonitoringObject is null || m2.MonitoringObject is null)
        return false;
      return m1.MonitoringObject.Id == m2.MonitoringObject.Id;
    }

    public static bool operator ==(BindableMonitoringObject m1, BindableMonitoringObject m2)
    {
      return InternalCompare(m1, m2);
    }
    public static bool operator !=(BindableMonitoringObject m1, BindableMonitoringObject m2)
    {
      return !InternalCompare(m1, m2);
    }
  }
}