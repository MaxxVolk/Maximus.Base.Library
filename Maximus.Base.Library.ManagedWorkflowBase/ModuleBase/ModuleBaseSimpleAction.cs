// .Net Framework
using System;
using System.Xml;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;
using Microsoft.EnterpriseManagement.Modules.DataItems.Discovery;
using Microsoft.EnterpriseManagement.Mom.Modules.DataItems;

namespace Maximus.Library.ManagedModuleBase
{
  /// <summary>
  /// Base class for any simple SCOM managed code workflow. Can implement ProbeAction and WriteAction. 
  /// Limited usage for ConditionDetection. Not to be used to implement DataSource.
  /// </summary>
  /// <typeparam name="TOutputDataType">Output data type for the an action. <seealso cref="PropertyBagDataItem"/> is the most
  /// common output type for a monitoring probe action. Use <seealso cref="DiscoveryDataItem"/> for a discovery
  /// probe action.</typeparam>
  public abstract class ModuleBaseSimpleAction<TOutputDataType> : ModuleBaseCore<TOutputDataType> where TOutputDataType : DataItemBase
  {
    /// <summary>
    /// Shall never create class instances directly.
    /// </summary>
    /// <param name="moduleHost"></param>
    /// <param name="configuration"></param>
    /// <param name="previousState"></param>
    public ModuleBaseSimpleAction(ModuleHost<TOutputDataType> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost, configuration, previousState)
    {
    }

    /// <summary>
    /// This method is called by SCOM Agent.
    /// </summary>
    public sealed override void Shutdown()
    {
      lock (shutdownLock)
      {
        shutdown = true;
        OnShutdownInvoke();
      }
    }

    /// <summary>
    /// This method is called by SCOM Agent.
    /// </summary>
    public sealed override void Start()
    {
      lock (shutdownLock)
      {
        if (shutdown)
          return;
        OnStartInvoke();
        ModuleHost.RequestNextDataItem();
      }
    }

    /// <summary>
    /// Override this method to perform module's main function.
    /// </summary>
    /// <param name="inputDataItems"></param>
    /// <returns></returns>
    protected abstract TOutputDataType[] GetOutputData(DataItemBase[] inputDataItems);

    /// <summary>
    /// This method is called by SCOM Agent.
    /// </summary>
    /// <param name="dataItems"></param>
    /// <param name="logicallyGrouped"></param>
    /// <param name="acknowledgeCallback"></param>
    /// <param name="acknowledgedState"></param>
    /// <param name="completionCallback"></param>
    /// <param name="completionState"></param>
    [InputStream(0)]
    public void OnNewDataItems(DataItemBase[] dataItems, bool logicallyGrouped, DataItemAcknowledgementCallback acknowledgeCallback, object acknowledgedState, DataItemProcessingCompleteCallback completionCallback, object completionState)
    {
      if (acknowledgeCallback == null && completionCallback != null)
        throw new ArgumentNullException(nameof(acknowledgeCallback), "Either both or none of completion and acknowledge callbacks must be specified.");
      if (acknowledgeCallback != null && completionCallback == null)
        throw new ArgumentNullException(nameof(completionCallback), "Either both or none of competition and acknowledge callbacks must be specified.");
      bool NeedAcknowledge = acknowledgeCallback != null;
      lock (shutdownLock)
      {
        if (shutdown)
          return;
        TOutputDataType[] ReturningResults = null;
        try
        {
          ReturningResults = GetOutputData(dataItems);
        }
        catch (Exception e)
        {
          try
          {
            string msg = $"Failed to get module's output data in {nameof(GetOutputData)} of {GetType().FullName}";
            ModuleErrorSignalReceiver(ModuleErrorSeverity.DataLoss, ModuleErrorCriticality.ThrowAndContinue, new ModuleBaseException(msg, e), msg);
          }
          catch { } // ignore
        }
        // if no data returned OR if exception thrown in GetOutputData(dataItems)
        if (ReturningResults == null || ReturningResults.Length == 0)
        {
          if (NeedAcknowledge)
          {
            acknowledgeCallback(acknowledgedState);
            completionCallback(completionState);
          }
          ModuleHost.RequestNextDataItem();
        }
        else if (NeedAcknowledge)
        {
          void AcknowledgeBypass(object ackState)
          {
            lock (shutdownLock)
            {
              if (shutdown)
                return;
              acknowledgeCallback(acknowledgedState);
              completionCallback(completionState);
              ModuleHost.RequestNextDataItem();
            }
          }
          if (ReturningResults.Length == 1)
            ModuleHost.PostOutputDataItem(ReturningResults[0], AcknowledgeBypass, null);
          else
            ModuleHost.PostOutputDataItems(ReturningResults, logicallyGrouped, AcknowledgeBypass, null);
        }
        else
        {
          if (ReturningResults.Length == 1)
            ModuleHost.PostOutputDataItem(ReturningResults[0]);
          else
            ModuleHost.PostOutputDataItems(ReturningResults, logicallyGrouped);
          ModuleHost.RequestNextDataItem();
        }
      }
    }
  }
}