// .Net Framework
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;

namespace Maximus.Library.ManagedModuleBase
{
  public abstract class ModuleBaseTimedAsyncDataSource<TOutputDataType> : ModuleBaseDataSource<TOutputDataType> where TOutputDataType : DataItemBase
  {
    private bool callbackActive;

    protected Timer DSTimer { get; private set; } = null;
    protected abstract long IntervalSeconds { get; }
    protected virtual long OnStartDelaySeconds { get; } = 0;


    public ModuleBaseTimedAsyncDataSource(ModuleHost<TOutputDataType> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost, configuration, previousState)
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
      if (callbackActive)
        return;
      Task.Run(async () =>
      {
        callbackActive = true;
        try
        {
          TOutputDataType[] result = await GetOutputDataAsync();
          if (result == null || shutdown)
            return;
          PostOutputData(result, null, false);
        }
        catch (Exception e)
        {
          try
          {
            string msg = $"Failed to get module's output data in {nameof(GetOutputDataAsync)} of {GetType().FullName}";
            ModuleErrorSignalReceiver(ModuleErrorSeverity.Warning, ModuleErrorCriticality.ThrowAndContinue, new ModuleBaseException(msg, e), msg);
          }
          catch { } // ignore
        }
        finally
        {
          callbackActive = false;
        }
      }
      );
    }

    private Task<TOutputDataType[]> GetOutputDataAsync()
    {
      return Task.Run(() => GetOutputData());
    }

    protected abstract TOutputDataType[] GetOutputData();
  }

}