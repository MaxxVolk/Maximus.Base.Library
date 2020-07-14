using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maximus.Library.Helpers
{
  public enum EventLoggingLevel { None = 0, Error = 1, Warning = 2, Informational = 3, Verbose = 4 }

  public class LoggingHelper
  {
    // internal fields
    private string _eventSourceName = "";
    private Dictionary<Type, int> _eventTypeIDs = new Dictionary<Type, int>();
    private object _eventTypeIDs_lock = new object();
    private const string SoftwareKeyName = "Maximus";

    public LoggingHelper(string eventSourceName, int eventBaseId, EventLoggingLevel eventLoggingLevel)
    {
      EventSourceName = eventSourceName;
      EventBaseID = eventBaseId;
      EventLoggingLevel = eventLoggingLevel;
    }

    #region Public Properties
    public string EventSourceName
    {
      get { return _eventSourceName; }
      set
      {
        if (_eventSourceName != value)
        {
          _eventSourceName = value;
          RegisterEventSource(value);
        }
      }
    }

    public int EventBaseID { get; set; }

    private EventLoggingLevel _eventLoggingLevel = EventLoggingLevel.Verbose;
    public EventLoggingLevel EventLoggingLevel
    {
      get
      {
        try
        {
          uint level = RegistryHelper.ReadRegistryUInt(".", $"HKLM:\\SOFTWARE\\{SoftwareKeyName}\\Management Packs\\{EventSourceName}\\Event Logging Level", 1024);
          if (level >= 0 && level <= 4)
            return (EventLoggingLevel)level;
        }
        catch { }
        return _eventLoggingLevel;
      }
      set => _eventLoggingLevel = value;
    }

    private string _DebugLogFolderPath = null;
    public string DebugLogFolderPath
    {
      get
      {
        try
        {
          return (RegistryHelper.ReadRegistryString(".", $"HKLM:\\SOFTWARE\\{SoftwareKeyName}\\Management Packs\\{EventSourceName}\\Debug Log Path", null) ?? _DebugLogFolderPath) ?? Environment.GetEnvironmentVariable("windir") + "\\debug";
        }
        catch
        {
          return _DebugLogFolderPath ?? Environment.GetEnvironmentVariable("windir") + "\\debug";
        }
      }
      set => _DebugLogFolderPath = value;
    }

    public bool DuplicateToVerboseChanned { get; set; } = true;
    #endregion

    public void AddLoggingSource(Type classType, int incrementalID)
    {
      lock (_eventTypeIDs_lock)
      {
        if (!_eventTypeIDs.ContainsKey(classType))
        {
          _eventTypeIDs.Add(classType, EventBaseID + incrementalID);
        }
      }
    }

    private void RegisterEventSource(string SourceName)
    {
#if (!DEBUG) // release only
      if (!EventLog.SourceExists(SourceName))
      {
        EventSourceCreationData sourceInfo = new EventSourceCreationData(SourceName, "Operations Manager");
        EventLog.CreateEventSource(sourceInfo);
      }
#endif
    }

    public void WriteVerbose(string filePath, string debugInfo, string component, string source, int thread, int nestedCalls = 1)
    {
      // example:
      // <![LOG[Message]LOG]!><time="14:11:02.040-780" date="03-27-2020" component="CcmExec" context="" type="1" thread="5400" file="powerstatemanager.cpp:1065">

      if (EventLoggingLevel < EventLoggingLevel.Verbose)
        return;

      DateTime localNow = DateTime.Now;
      DateTime utcNow = DateTime.UtcNow;
      int UTCOffset = (int)Math.Truncate(utcNow.Subtract(localNow).TotalMinutes);
      string strUTCOffset;
      if (UTCOffset < 0)
        strUTCOffset = UTCOffset.ToString("D");
      else
        strUTCOffset = "+" + UTCOffset.ToString("D");
      string formattedMessage = "<![LOG[" + debugInfo + "]LOG]!>";
      // time="17:23:36.867-720" date="04-20-2016"
      string strTime = "time=\"" + localNow.ToString("HH:mm:ss.fff") + strUTCOffset + "\"";
      string strDate = "date=\"" + localNow.ToString("MM-dd-yyyy") + "\"";
      string strLogFileName = Path.Combine(filePath, $"{_eventSourceName ?? "empty"}-{localNow.ToString("yyyy-MM-dd")}.log");
      if (string.IsNullOrEmpty(component) || string.IsNullOrEmpty(source))
      {
        StackFrame lastFrame = new StackTrace(nestedCalls, true).GetFrame(0);
        component = lastFrame.GetMethod().Name;
        source = lastFrame.GetFileName() + ":" + lastFrame.GetFileLineNumber();
      }
      formattedMessage += "<" + strTime + " " + strDate + " component=\"" + component +
        "\" context=\"\" type=\"1\" thread=\"" + thread.ToString() + "\" file=\"" + source + "\">" + "\r\n";
      // need that due to multi-threading nature
      for (int attemptCounter = 0; attemptCounter < 10; attemptCounter++)
      {
        try
        {
          File.AppendAllText(strLogFileName, formattedMessage);
          break; // exit the loop if the write op is successful
        }
        catch (Exception)
        {
          Thread.Sleep(1); // wait a bit and repeat
        }
      }
    }

    public void WriteVerbose(string debugInfo, string component, string source, int thread, int nestedCalls = 1)
    {
      WriteVerbose(DebugLogFolderPath, debugInfo, component, source, thread, nestedCalls);
    }

    public void WriteInformation(string message, object Src, params object[] stringFormatArgs)
    {
#if DEBUG // using only text file logging in DEBUG release
      WriteVerbose(message, "", "", 0);
#else
      if (EventLoggingLevel >= EventLoggingLevel.Informational)
        WriteEvent(message, EventLogEntryType.Information, Src, stringFormatArgs);
#endif
    }
    public void WriteWarning(string message, object Src, params object[] stringFormatArgs)
    {
#if DEBUG // using only text file logging in DEBUG release
      WriteVerbose(message, "", "", 1);
#else
      if (EventLoggingLevel >= EventLoggingLevel.Warning)
        WriteEvent(message, EventLogEntryType.Warning, Src, stringFormatArgs);
#endif
    }
    public void WriteError(string message, object Src, params object[] stringFormatArgs)
    {
#if DEBUG // using only text file logging in DEBUG release
      WriteVerbose(message, "", "", 2);
#else
      if (EventLoggingLevel >= EventLoggingLevel.Error)
        WriteEvent(message, EventLogEntryType.Error, Src, stringFormatArgs);
#endif
    }

    public void WriteException(string message, Exception e, object Src, params object[] stringFormatArgs)
    {
      string exceptionDescription = "";
      exceptionDescription += "Exception in " + (Src?.GetType()?.Name ?? "N/A") + ".\r\n";
      exceptionDescription += "Message: " + (message ?? "<NULL message>") + "\r\n\r\n";
      exceptionDescription += "Exceptions:\r\n";
      Exception loopException = e;
      int ordernum = 1;
      do
      {
        exceptionDescription += ordernum.ToString() + "): Exception type: " + loopException.GetType().Namespace + "." + loopException.GetType().Name + "\r\n";
        exceptionDescription += loopException.GetType().FullName + " exception (" + loopException.Message + ")";
        exceptionDescription += loopException.StackTrace + "\r\n\r\n";
        loopException = loopException.InnerException;
        ordernum++;
      } while (loopException != null);
      WriteError(exceptionDescription, Src, stringFormatArgs);
    }

    public void WriteException(Exception e, object Src, params object[] stringFormatArgs)
    {
      StackTrace stackTrace = new StackTrace();
      MethodBase callingMethod = stackTrace.GetFrame(1).GetMethod();
      string message = "Generic exception in the " + callingMethod.Name +
        " method of the " + callingMethod.DeclaringType.Name + " class.";
      WriteException(message, e, Src, stringFormatArgs);
    }

    private int _cachedPID = -1;
    private void WriteEvent(string message, EventLogEntryType type, object Src, params object[] formatArgs)
    {
      // it's not referenced in DEBUG release
      try
      {
        if (formatArgs != null && formatArgs.Length > 0)
          message = string.Format(message, formatArgs);
        if (_cachedPID == -1)
          _cachedPID = Process.GetCurrentProcess().Id;
        else
          message += "\r\n\r\nCurrent Process Id (PID): " + _cachedPID.ToString();
        if (Src == null)
        {
          EventLog.WriteEntry(_eventSourceName, message, type, EventBaseID);
          if (DuplicateToVerboseChanned)
            WriteVerbose(message, null, null, _cachedPID, 3);
        }
        else if (Src.GetType() == typeof(Type))
        {
          EventLog.WriteEntry(_eventSourceName, message, type, EventIdFromSource((Type)Src));
          if (DuplicateToVerboseChanned)
            WriteVerbose(message, null, null, _cachedPID, 3);
        }
        else
        {
          try
          {
            Type srcType = Src.GetType();
            PropertyInfo moduleNamePropertyInfo = srcType.GetProperty("ModuleName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            string moduleNameValue = "<ModuleName peoperty is not defined>";
            if (moduleNamePropertyInfo != null)
              moduleNameValue = FormattingHelper.NullToEmptyString(moduleNamePropertyInfo.GetValue(Src, null));
            else
              moduleNameValue = Src.GetType().FullName;
            PropertyInfo moduleVersionPropertyInfo = srcType.GetProperty("ModuleVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            string moduleVersionValue = "<ModuleVersion peoperty is not defined>";
            if (moduleVersionPropertyInfo != null)
              moduleVersionValue = FormattingHelper.NullToEmptyString(moduleVersionPropertyInfo.GetValue(Src, null));
            message += "\r\n\r\nModule: " + moduleNameValue + "\r\nVersion: " + moduleVersionValue;
          }
          catch { message += "\r\n\r\nModule: <not available>\r\nVersion: <not available>"; } // do nothing
          EventLog.WriteEntry(_eventSourceName, message, type, EventIdFromSource(Src.GetType()));
          if (DuplicateToVerboseChanned)
            WriteVerbose(message, null, null, _cachedPID, 3);
        }
      }
      catch (Exception e)
      {
        // shall fail to file logging if any issues with Event Log
        if (Src != null)
          WriteVerbose(message + "\r\n" + e.Message, Src.GetType().Name, "", (int)type);
        else
          WriteVerbose(message + "\r\n" + e.Message, "", "", (int)type);
      }
    }

    private int EventIdFromSource(Type src)
    {
      lock (_eventTypeIDs_lock)
      {
        if (_eventTypeIDs.ContainsKey(src))
          return _eventTypeIDs[src];
        else
          return EventBaseID;
      }
    }
  }
}
