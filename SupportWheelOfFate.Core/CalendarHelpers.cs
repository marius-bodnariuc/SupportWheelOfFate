using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public static class CalendarHelpers
    {
        public static IEnumerable<DateTime> GetWorkdaysInMonth(int month)
        {
            return Enumerable.Range(1, GetDaysInMonth(month))
                .Select(day => new DateTime(DateTime.Today.Year, month, day))
                .Where(date => Workdays.Contains(date.DayOfWeek));
        }

        public static int GetWorkWeeksInMonth(int month)
        {
            return GetDaysInMonth(month) / Workdays.Count() + 1;
        }

        public static int GetDaysInMonth(int month)
        {
            return DateTime.DaysInMonth(DateTime.Today.Year, month);
        }

        private static IEnumerable<DayOfWeek> Workdays =>
            new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };
    }
}
