using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SupportWheelOfFate.Core.Test
{
    public class NaiveMonthlyScheduleGeneratorTests
    {
        [Fact]
        public void GeneratesValidSchedulesToCoverAllWorkingDaysInMonth()
        {
            var monthlyScheduleGenerator = new NaiveMonthlyScheduleGenerator(People);

            var schedules = monthlyScheduleGenerator.Generate();

            Assert.Equal(WorkdaysInMonth.Count() * 2, schedules.Count());
        }

        private IEnumerable<string> People =>
            new[]
            {
                "John",
                "Jane",
                "Jack",
                "Jim",
                "Jacqueline",
                "Jenny",
                "Jeroen",
                "Joel",
                "Jojo",
                "Jasmine"
            };

        private IEnumerable<DateTime> WorkdaysInMonth =>
            Enumerable.Range(1, DaysInMonth)
                .Select(day => new DateTime(DateTime.Today.Year, DateTime.Today.Month, day))
                .Where(date => Workdays.Contains(date.DayOfWeek));

        private int DaysInMonth => DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

        private IEnumerable<DayOfWeek> Workdays =>
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
