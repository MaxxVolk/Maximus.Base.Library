// .Net Framework
using System;
using System.Threading;
using System.Xml;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;

namespace Maximus.Library.ManagedModuleBase
{
  /// <summary>
  /// Base Data Source class. Implements basic logic and methods to use in Data Source.
  /// </summary>
  /// <typeparam name="TOutputDataType"></typeparam>
  public abstract class ModuleBaseTimedDataSource<TOutputDataType> : ModuleBaseDataSource<TOutputDataType> where TOutputDataType : DataItemBase
  {
    private bool callbackActive = false;

    protected Timer DSTimer { get; private set; } = null;
    protected abstract long IntervalSeconds { get; }
    protected virtual long OnStartDelaySeconds { get; } = 0;

    public ModuleBaseTimedDataSource(ModuleHost<TOutputDataType> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost, configuration, previousState)
    {

      OnStart += ModuleBaseTimedDataSource_OnStart;
      OnShutdown += ModuleBaseTimedDataSource_OnShutdown;
    }

    private void ModuleBaseTimedDataSource_OnShutdown(object sender, EventArgs e)
    {
      if (DSTimer != null)
        DSTimer.Dispose();
      DSTimer = null;
    }

    private void ModuleBaseTimedDataSource_OnStart(object sender, EventArgs e)
    {
      DSTimer = new Timer(TimerCallback, this, OnStartDelaySeconds, IntervalSeconds * 1000);
    }

    private void TimerCallback(object state)
    {
      if (shutdown)
        return;
      if (callbackActive)
      {
        try
        {
          string msg = $"{GetType().FullName} cannot start {nameof(GetOutputData)} callback, because the previous one is still running.";
          ModuleErrorSignalReceiver(ModuleErrorSeverity.Warning, ModuleErrorCriticality.ThrowAndContinue, new ModuleException(msg), msg);
        } catch { }
        return;
      }
      callbackActive = true;
      try
      {
        TOutputDataType[] result = GetOutputData();
        if (result == null || shutdown)
          return;
        PostOutputData(result, null, false);
      }
      catch (Exception e)
      {
        try
        {
          string msg = $"Failed to get output data from {GetType().FullName}. Exception in GetOutputData.";
          ModuleErrorSignalReceiver(ModuleErrorSeverity.DataLoss, ModuleErrorCriticality.ThrowAndContinue, new ModuleException(msg, e), msg);
        } catch { } // ignore
      }
      finally
      {
        callbackActive = false;
      }
    }

    protected abstract TOutputDataType[] GetOutputData();
  }

}