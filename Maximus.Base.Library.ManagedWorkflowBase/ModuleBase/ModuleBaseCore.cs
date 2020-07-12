// .Net Framework
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Maximus.Library.Helpers;

// SCOM SDK
using Microsoft.EnterpriseManagement.HealthService;

namespace Maximus.Library.ManagedModuleBase
{
  /// <summary>
  /// Adds core workflow and abstract initialization procedures. No data handling defined. 
  /// </summary>
  /// <typeparam name="TOutputDataType"></typeparam>
  public abstract class ModuleBaseCore<TOutputDataType> : ModuleBaseWithHelpers<TOutputDataType> where TOutputDataType : DataItemBase
  {
    protected readonly object shutdownLock;
    protected bool shutdown;
    private bool? internal_IsStateless = null;

    public ModuleBaseCore(ModuleHost<TOutputDataType> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof(configuration));
      shutdownLock = new object();
      PreInitialize(moduleHost, configuration, previousState);
      LoadConfiguration(configuration);
      LoadPreviousState(previousState);
    }

    protected bool IsStateless
    {
      get
      {
        if (internal_IsStateless != null)
          return internal_IsStateless == true;
        StatelessModuleAttribute attribute = (StatelessModuleAttribute)Attribute.GetCustomAttribute(GetType(), typeof(StatelessModuleAttribute));
        if (attribute == null)
          internal_IsStateless = true; // module is stateless by default if no attribute is defined
        else
          internal_IsStateless = attribute.IsStateless; // else use attribute value
        return internal_IsStateless == true;
      }
    }
      
    protected object ModuleState { get; set; }

    protected virtual void LoadPreviousState(byte[] previousState)
    {
      if (previousState != null)
        try
        {
          using (MemoryStream memoryStream = new MemoryStream(previousState))
          {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            try
            {
              ModuleState = binaryFormatter.Deserialize(memoryStream);
            }
            catch
            {
              ModuleState = null;
            }
          }
        }
        catch (Exception e)
        {
          try
          {
            string msg = "Failed to load module's previous state. Empty state is used.";
            ModuleErrorSignalReceiver(ModuleErrorSeverity.Warning, ModuleErrorCriticality.Continue, new ModuleBaseException(msg, e), msg);
          }
          catch { } // ignore
        }
    }

    /// <summary>
    /// Tries to serialize the current state object held in <code>ModuleState</code> property and save it using SCOM Agent API.
    /// </summary>
    /// <returns>Returns true if success, false otherwise.</returns>
    protected bool SavePreviousState()
    {
      if (ModuleState == null)
        return false;
      lock (shutdownLock)
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          try
          {
            binaryFormatter.Serialize(memoryStream, ModuleState);
            ModuleHost.SaveState(memoryStream.GetBuffer(), (int)memoryStream.Length);
            return true;
          }
          catch (Exception e)
          {
            try
            {
              string msg = "Failed to save module's previous state.";
              ModuleErrorSignalReceiver(ModuleErrorSeverity.DataLoss, ModuleErrorCriticality.Continue, new ModuleBaseException(msg, e), msg);
            }
            catch { } // ignore
            return false;
          }
        }
      }
    }

    protected bool SavePreviousState(long bookmark)
    {
      ModuleState = new StateBookmarks { longBookmark = bookmark };
      return SavePreviousState();
    }

    protected bool SavePreviousState(DateTime bookmark)
    {
      ModuleState = new StateBookmarks { timeBookmark = bookmark };
      return SavePreviousState();
    }

    protected bool SavePreviousState(double bookmark)
    {
      ModuleState = new StateBookmarks { doubleBookmark = bookmark };
      return SavePreviousState();
    }

    protected bool SavePreviousState(object state)
    {
      ModuleState = state;
      return SavePreviousState();
    }

    protected abstract void LoadConfiguration(XmlReader configuration);

    protected event EventHandler OnStart;

    protected event EventHandler OnShutdown;

    protected virtual void OnStartInvoke()
    {
      OnStart?.Invoke(this, null);
    }
    protected virtual void OnShutdownInvoke()
    {
      OnShutdown?.Invoke(this, null);
    }

    /// <summary>
    /// Inherited classes may override this method to make an earlier, before the most base constructor, initialization. 
    /// For example, to setup logging parameters.
    /// </summary>
    /// <param name="moduleHost"></param>
    /// <param name="configuration"></param>
    /// <param name="previousState"></param>
    protected virtual void PreInitialize(ModuleHost<TOutputDataType> moduleHost, XmlReader configuration, byte[] previousState)
    {
    }
  }
}