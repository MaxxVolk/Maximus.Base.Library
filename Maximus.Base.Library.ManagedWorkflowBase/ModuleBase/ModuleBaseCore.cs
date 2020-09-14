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

    /// <summary>
    /// Shall never create class instances directly.
    /// </summary>
    /// <param name="moduleHost"></param>
    /// <param name="configuration"></param>
    /// <param name="previousState"></param>
    public ModuleBaseCore(ModuleHost<TOutputDataType> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof(configuration));
      shutdownLock = new object();
      PreInitialize(moduleHost, configuration, previousState);
      XmlDocument xmlDocument = new XmlDocument();
      if (configuration != null)
        xmlDocument.Load(configuration);
      LoadConfiguration(xmlDocument);
      LoadPreviousState(previousState);
    }

    /// <summary>
    /// Returns value of the <seealso cref="StatelessModuleAttribute"/> attribute, or <c>false</c> if no attribute. 
    /// </summary>
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

    /// <summary>
    /// A property to hold module state after loading at startup or before calling <see cref="SavePreviousState()"/> method.
    /// <see cref="ModuleState"/> is null if: no saved state provided, module version has changes, target class instance migrated to another agent.
    /// </summary>
    protected object ModuleState { get; set; }

    /// <summary>
    /// Automatically called in module's constructor at startup. Tries to load previous module state if provided.
    /// If successfully loaded, previous module state is held in the <see cref="ModuleState"/> property.
    /// </summary>
    /// <param name="previousState"></param>
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
    /// Tries to serialize the current state object held in <see cref="ModuleState"/> property and save it using SCOM Agent API.
    /// </summary>
    /// <returns>Returns <c>true</c> if success, false otherwise.</returns>
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

    /// <summary>
    /// Assign <paramref name="bookmark"/> to <see cref="ModuleState"/> property and save it.
    /// </summary>
    /// <param name="bookmark"></param>
    /// <returns><c>true</c> if saved successfully.</returns>
    protected bool SavePreviousState(long bookmark)
    {
      ModuleState = new StateBookmarks { longBookmark = bookmark };
      return SavePreviousState();
    }

    /// <summary>
    /// Assign <paramref name="bookmark"/> to <see cref="ModuleState"/> property and save it.
    /// </summary>
    /// <param name="bookmark"></param>
    /// <returns><c>true</c> if saved successfully.</returns>
    protected bool SavePreviousState(DateTime bookmark)
    {
      ModuleState = new StateBookmarks { timeBookmark = bookmark };
      return SavePreviousState();
    }

    /// <summary>
    /// Assign <paramref name="bookmark"/> to <see cref="ModuleState"/> property and save it.
    /// </summary>
    /// <param name="bookmark"></param>
    /// <returns><c>true</c> if saved successfully.</returns>
    protected bool SavePreviousState(double bookmark)
    {
      ModuleState = new StateBookmarks { doubleBookmark = bookmark };
      return SavePreviousState();
    }

    /// <summary>
    /// Assign <paramref name="state"/> to <see cref="ModuleState"/> property and save it.
    /// </summary>
    /// <param name="state"></param>
    /// <returns><c>true</c> if saved successfully.</returns>
    protected bool SavePreviousState(object state)
    {
      ModuleState = state;
      return SavePreviousState();
    }

    protected abstract void LoadConfiguration(XmlDocument cfgDoc);

    /// <summary>
    /// Event is triggered when SCOM Agent starts the module for the first time.
    /// </summary>
    protected event EventHandler OnStart;

    /// <summary>
    /// Event is triggered when SCOM Agent do graceful shutdown of the module.
    /// This happens when: <list type="bullet"><item>dd</item></list>
    /// </summary>
    protected event EventHandler OnShutdown;

    protected void OnStartInvoke() => OnStart?.Invoke(this, null);

    protected void OnShutdownInvoke() => OnShutdown?.Invoke(this, null);

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