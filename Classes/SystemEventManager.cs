using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace WOTTracker
{
    // Dans SystemEventManager.cs
    public static class SystemEventManager
    {
        public static List<SystemEvent> GetEventsSince(DateTime startTime)
        {
            var events = new List<SystemEvent>();
            string query = $@"
            *[System[
            (EventID=1 or EventID=42 or EventID=12 or EventID=13 or EventID=506 or EventID=507)
            and TimeCreated[@SystemTime >= '{startTime.ToUniversalTime().ToString("o")}']]]";


            try
            {
                EventLogReader logReader = new EventLogReader(new EventLogQuery("System", PathType.LogName, query));
                for (EventRecord r = logReader.ReadEvent(); r != null; r = logReader.ReadEvent())
                {
                    if (r.TimeCreated.Value.ToLocalTime() > startTime) // On s'assure de ne pas reprendre le dernier événement
                    {
                        events.Add(new SystemEvent { Timestamp = r.TimeCreated.Value.ToLocalTime(), EventId = r.Id, EventType = GetEventTypeFromId(r.Id) });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ERROR: Unable to read the event observer. The application must be run as an administrator.");
            }
            return events
            .OrderBy(e => e.Timestamp)
            .GroupBy(e => new DateTime(e.Timestamp.Year, e.Timestamp.Month, e.Timestamp.Day, e.Timestamp.Hour, e.Timestamp.Minute, 0))
            .Select(g => g.First()) // garde le premier event de chaque minute
            .ToList();
        }

        private static string GetEventTypeFromId(long eventId)
        {
            switch (eventId)
            {
                case 1:
                case 507:
                    return "Réveil";
                case 12: return "Démarrage";
                case 13: return "Arrêt";
                case 42:
                case 506: return "Veille";
                default: return "Inconnu";
            }
        }
    }
}


