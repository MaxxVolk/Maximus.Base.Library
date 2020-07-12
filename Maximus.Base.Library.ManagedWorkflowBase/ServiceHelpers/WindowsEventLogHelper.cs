using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Xml;

namespace Maximus.Library.Helpers
{
  public static class WindowsEventLogHelper
  {
    public static EventBookmark GetLatestEventBookmark(this EventLogSession eventLogSession, string eventLogName)
    {
      EventLogQuery eventsQuery = new EventLogQuery(eventLogName, PathType.LogName)
      {
        TolerateQueryErrors = true,
        Session = eventLogSession,
        ReverseDirection = true
      };
      using (EventLogReader logReader = new EventLogReader(eventsQuery))
      {
        using (EventRecord logEvent = logReader.ReadEvent())
          return logEvent?.Bookmark;
      }
    }

    public static EventLogReader ReadLatestEventsFromBookmark(this EventLogSession eventLogSession, string eventLogName, EventBookmark lastPosition, string xPathQuery = null)
    {
      EventLogQuery eventsQuery = xPathQuery == null ? new EventLogQuery(eventLogName, PathType.LogName)
      {
        TolerateQueryErrors = true,
        Session = eventLogSession,
        ReverseDirection = false
      } 
      :
      new EventLogQuery(eventLogName, PathType.LogName, xPathQuery)
      {
        TolerateQueryErrors = true,
        Session = eventLogSession,
        ReverseDirection = false
      };
      return new EventLogReader(eventsQuery, lastPosition);
    }

    public static EventBookmark ReadIncrementalEventsFromBookmark(this EventLogSession eventLogSession, string eventLogName, EventBookmark lastPosition, string xPathQuery, out IList<EventRecord> eventRecords)
    {
      EventBookmark latestEvent = eventLogSession.GetLatestEventBookmark(eventLogName);
      using (EventLogReader eventLogReader = eventLogSession.ReadLatestEventsFromBookmark(eventLogName, lastPosition, xPathQuery))
      {
        eventRecords = new List<EventRecord>();
        EventRecord currentEvent = eventLogReader.ReadEvent();
        EventBookmark latestReadEvent = null;
        while (currentEvent != null)
        {
          eventRecords.Add(currentEvent);
          latestReadEvent = currentEvent.Bookmark;
          currentEvent = eventLogReader.ReadEvent();
        }
        return latestReadEvent ?? latestEvent;
      }
    }

    public static EventBookmark ReadIncrementalEventsFromBookmark(this EventLogSession eventLogSession, string eventLogName, EventBookmark lastPosition, string xPathQuery, out IList<XmlDocument> eventRecords)
    {
      EventBookmark latestEvent = eventLogSession.GetLatestEventBookmark(eventLogName);
      using (EventLogReader eventLogReader = eventLogSession.ReadLatestEventsFromBookmark(eventLogName, lastPosition, xPathQuery))
      {
        eventRecords = new List<XmlDocument>();
        EventRecord currentEvent = eventLogReader.ReadEvent();
        EventBookmark latestReadEvent = null;
        while (currentEvent != null)
        {
          using (currentEvent)
          {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(currentEvent.ToXml());
            eventRecords.Add(xmlDocument);
            latestReadEvent = currentEvent.Bookmark;
          }
          currentEvent = eventLogReader.ReadEvent();
        }
        return latestReadEvent ?? latestEvent;
      }
    }

    public static void Dispose(this IList<EventRecord> eventRecords)
    {
      foreach (EventRecord eventRecord in eventRecords)
        eventRecord.Dispose();
      eventRecords.Clear();
    }

    public static string GetXPathEventQuery(string logName, int eventId)
    {
      return $"<QueryList><Query Id=\"0\" Path=\"{logName}\"><Select Path=\"{logName}\">*[System[(EventID={eventId.ToString()})]]</Select></Query></QueryList>";
    }

    public static string GetXPathEventQuery(string logName, int[] eventIdList)
    {
      if (eventIdList == null || eventIdList.Length == 0)
        throw new ArgumentOutOfRangeException("eventIdList");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append($"<QueryList><Query Id=\"0\" Path=\"{logName}\"><Select Path=\"{logName}\">*[System[(EventID={eventIdList[0].ToString()}");
      for (int i = 1; i < eventIdList.Length; i++)
        stringBuilder.Append($" or EventID={eventIdList[i].ToString()}");
      stringBuilder.Append(")]]</Select></Query></QueryList>");
      return stringBuilder.ToString();
    }
  }
}