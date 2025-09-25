using System;
using System.Collections.Generic;

namespace WOTTracker.Models
{
    public class OvertimeByDayDto
    {
        public DayOfWeek Day { get; }
        public int Minutes { get; }

        public OvertimeByDayDto(DayOfWeek day, int minutes)
        {
            Day = day;
            Minutes = minutes;
        }
    }

    public class OvertimeBreakdownDto
    {
        public int NormalDays { get; }
        public int Saturdays { get; }
        public int Sundays { get; }
        public int PublicHolidays { get; }
        public int Total { get; }

        public OvertimeBreakdownDto(int normalDays, int saturdays, int sundays, int publicHolidays, int total)
        {
            NormalDays = normalDays;
            Saturdays = saturdays;
            Sundays = sundays;
            PublicHolidays = publicHolidays;
            Total = total;
        }
    }

    public class LongestSessionDto
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public double Hours { get; }
        public string WorkingType { get; }

        public LongestSessionDto(DateTime start, DateTime end, double hours, string workingType)
        {
            Start = start;
            End = end;
            Hours = hours;
            WorkingType = workingType;
        }
    }

    public class BurnoutWeekDto
    {
        public int Year { get; }
        public int IsoWeek { get; }
        public double OvertimeHours { get; }

        public BurnoutWeekDto(int year, int isoWeek, double overtimeHours)
        {
            Year = year;
            IsoWeek = isoWeek;
            OvertimeHours = overtimeHours;
        }
    }

    public class BurnoutRiskResult
    {
        public bool IsAtRisk { get; }
        public List<BurnoutWeekDto> ConsecutiveWeeksOver10Hours { get; }

        public BurnoutRiskResult(bool isAtRisk, List<BurnoutWeekDto> consecutiveWeeksOver10Hours)
        {
            IsAtRisk = isAtRisk;
            ConsecutiveWeeksOver10Hours = consecutiveWeeksOver10Hours ?? new List<BurnoutWeekDto>();
        }
    }

}
