using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTTracker.Models
{
    public class StatsService
    {
        private readonly string _dbPath;

        public StatsService(string dbPath)
        {
            _dbPath = dbPath;
        }

        public OvertimeBreakdownDto GetOvertimeBreakdown()
        {
            using var db = new YotTrackerDbContext(_dbPath);

            var now = DateTime.Now;
            var validWs = db.WorkSessions
                .Where(s => db.AppSettingsHistory.Any(a =>
                    a.VersionId == s.AppSettingsVersionId && a.ExpirationOverTime > now));

            int normal = validWs.Where(s => s.TypeOverTime == "NormalDays").Sum(s => s.OverTimeMinutes ?? 0);
            int sat = validWs.Where(s => s.TypeOverTime == "Saturdays").Sum(s => s.OverTimeMinutes ?? 0);
            int sun = validWs.Where(s => s.TypeOverTime == "Sundays").Sum(s => s.OverTimeMinutes ?? 0);
            int hol = validWs.Where(s => s.TypeOverTime == "PublicHolidays").Sum(s => s.OverTimeMinutes ?? 0);

            int total = normal + sat + sun + hol;
            return new OvertimeBreakdownDto(normal, sat, sun, hol, total);
        }

        public List<OvertimeByDayDto> GetMostCommonOvertimeDays()
        {
            using var db = new YotTrackerDbContext(_dbPath);

            var sessions = db.WorkSessions
                .Where(s => (s.OverTimeMinutes ?? 0) > 0 && s.EndTime != null)
                .Select(s => new { s.StartTime, s.OverTimeMinutes })
                .ToList();

            return sessions
                .GroupBy(s => s.StartTime.DayOfWeek)
                .Select(g => new OvertimeByDayDto(g.Key, g.Sum(x => x.OverTimeMinutes ?? 0)))
                .OrderByDescending(x => x.Minutes)
                .ToList();
        }

        public List<LongestSessionDto> GetLongestSessions(int topN = 5)
        {
            using var db = new YotTrackerDbContext(_dbPath);

            return db.WorkSessions
                .Where(s => s.EndTime != null)
                .Select(s => new LongestSessionDto(
                    s.StartTime,
                    s.EndTime.Value,
                    (s.EndTime.Value - s.StartTime).TotalHours,
                    s.WorkingType
                ))
                .OrderByDescending(x => x.Hours)
                .Take(topN)
                .ToList();
        }

        public BurnoutRiskResult GetBurnoutRisk(double thresholdHoursPerWeek = 10.0, int consecutiveWeeks = 3)
        {
            using var db = new YotTrackerDbContext(_dbPath);

            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var cal = culture.Calendar;
            var rule = System.Globalization.CalendarWeekRule.FirstFourDayWeek;
            var firstDay = DayOfWeek.Monday;

            var weekly = db.WorkSessions
                .Where(s => (s.OverTimeMinutes ?? 0) > 0)
                .AsEnumerable() // GroupBy sur client car ISO week pas transcrit en SQL facilement
                .GroupBy(s => new {
                    Year = s.StartTime.Year,
                    Week = cal.GetWeekOfYear(s.StartTime, rule, firstDay)
                })
                .Select(g => new BurnoutWeekDto(
                    g.Key.Year,
                    g.Key.Week,
                    g.Sum(s => (s.OverTimeMinutes ?? 0) / 60.0)
                ))
                .OrderBy(w => w.Year).ThenBy(w => w.IsoWeek)
                .ToList();

            var listConsec = new List<BurnoutWeekDto>();
            var current = new List<BurnoutWeekDto>();

            for (int i = 0; i < weekly.Count; i++)
            {
                var w = weekly[i];
                if (w.OvertimeHours > thresholdHoursPerWeek)
                {
                    if (current.Count == 0 || IsNextWeek(current.Last(), w))
                        current.Add(w);
                    else
                    {
                        if (current.Count >= consecutiveWeeks)
                            listConsec.AddRange(current);
                        current = new List<BurnoutWeekDto> { w };
                    }
                }
                else
                {
                    if (current.Count >= consecutiveWeeks)
                        listConsec.AddRange(current);
                    current.Clear();
                }
            }
            if (current.Count >= consecutiveWeeks)
                listConsec.AddRange(current);

            return new BurnoutRiskResult(listConsec.Any(), listConsec);
        }

        private bool IsNextWeek(BurnoutWeekDto previous, BurnoutWeekDto current)
        {
            // naïf : même année et week + 1 ; si tu veux gérer le passage d'année, adapte
            return (current.Year == previous.Year && current.IsoWeek == previous.IsoWeek + 1)
                   || (current.Year == previous.Year + 1 && previous.IsoWeek >= 52 && current.IsoWeek == 1);
        }
    }

}
