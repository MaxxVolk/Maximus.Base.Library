// .Net Framework
using System;
using System.Xml;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;

namespace Maximus.Library.ManagedModuleBase
{
  public abstract class ModuleBaseDataSource<TOutputDataType> : ModuleBaseCore<TOutputDataType> where TOutputDataType : DataItemBase
  {
    public ModuleBaseDataSource(ModuleHost<TOutputDataType> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost, configuration, previousState)
    {
    }

    protected void PostOutputData(TOutputDataType[] ReturningResults, DataItemAcknowledgementCallback AcknowledgementCallback, bool LogicallyGrouped)
    {
      lock (shutdownLock)
      {
        if (shutdown)
          return;
      }
      try
      {
        if (ReturningResults != null || ReturningResults.Length > 0)
        {
          if (AcknowledgementCallback == null)
          {
            if (ReturningResults.Length == 1)
              ModuleHost.PostOutputDataItem(ReturningResults[0]);
            else
              ModuleHost.PostOutputDataItems(ReturningResults, LogicallyGrouped);
          }
          else
          {
            if (ReturningResults.Length == 1)
              ModuleHost.PostOutputDataItem(ReturningResults[0], AcknowledgementCallback, null);
            else
              ModuleHost.PostOutputDataItems(ReturningResults, LogicallyGrouped, AcknowledgementCallback, null);
          }
        }
      }
      catch (Exception e)
      {
        try
        {
          string msg = $"Failed to post output data back to the module host in {GetType().FullName}. Notification of data loss is sent the the module host.";
          ModuleErrorSignalReceiver(ModuleErrorSeverity.DataLoss, ModuleErrorCriticality.ThrowAndContinue, new ModuleBaseException(msg, e), msg);
        }
        catch { } // ignore
        ModuleHost.NotifyError(ModuleErrorSeverity.DataLoss, e);
      }
    }

    public override void Start()
    {
      // DO NOT call the base
      lock (shutdownLock)
      {
        if (shutdown)
          return;
        OnStartInvoke();
      }
    }

    public override void Shutdown()
    {
      lock (shutdownLock)
      {
        shutdown = true;
        OnShutdownInvoke();
      }
    }
  }
}