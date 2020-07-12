using System;
using System.Management;

namespace Maximus.Library.Helpers
{
  public class WMIQuery : IDisposable
  {
    protected ManagementScope queryScope;
    protected ObjectQuery query;
    protected ManagementObjectSearcher moSearcher = null;
    protected ManagementObjectCollection moCollection = null;

    public WMIQuery(string wqlQuery)
    {
      DefaultConstructor(WMIHelper.LocalManagementScope, wqlQuery);
    }

    public WMIQuery(ManagementScope managementScope, string wqlQuery)
    {
      DefaultConstructor(managementScope, wqlQuery);
    }

    protected void DefaultConstructor(ManagementScope managementScope, string wqlQuery)
    {
      queryScope = managementScope;
      query = new ObjectQuery(wqlQuery);
    }

    public ManagementObjectCollection Select()
    {
      if (!ManagementScope.IsConnected)
        ManagementScope.Connect();
      if (!disposedValue) // for repetitive query
        Dispose(false);
      try
      {
        moSearcher = new ManagementObjectSearcher(ManagementScope, Query);
        moCollection = moSearcher.Get();
      }
      finally
      {
        disposedValue = false;
      }
      return moCollection;
    }

    public ManagementScope ManagementScope => queryScope;
    public ObjectQuery Query => query;

    #region IDisposable Support
    private bool disposedValue = true; // no unmanaged resources allocated by default

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (moCollection != null)
          moCollection.Dispose();
        if (moSearcher != null)
          moSearcher.Dispose();

        disposedValue = true;
      }
    }

    ~WMIQuery()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
    }
    #endregion
  }
}