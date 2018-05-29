using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class NaiveMonthlyScheduleGenerator
    {
        private readonly IEnumerable<string> _people;

        public NaiveMonthlyScheduleGenerator(IEnumerable<string> people)
        {
            _people = people;
        }

        public IEnumerable<Schedule> Generate()
        {
            var schedules = WorkdaysInMonth.Zip(EmployeePairs, BuildSchedulesForEmployeePair)
                .SelectMany(schedule => schedule);

            return schedules;
        }

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

        private IEnumerable<(string, string)> EmployeePairs
        {
            get
            {
                var employeePairs = Enumerable.Empty<(string, string)>();
                (DaysInMonth / Workdays.Count() + 1).Times(() =>
                {
                    // making sure the first pair in a new set doesn't include
                    // any of the employees from the last pair in the previous set
                    var randomPairs = GetValidNextRandomPairList(employeePairs);

                    employeePairs = employeePairs.Concat(randomPairs);
                });

                return employeePairs;
            }
        }

        private static Func<DateTime, (string, string), List<Schedule>> BuildSchedulesForEmployeePair =>
            (workday, employeePair) => new List<Schedule>
                {
                    new Schedule
                    {
                        Employee = employeePair.Item1,
                        StartTime = new DateTime(workday.Year, workday.Month, workday.Day, 9, 0, 0),
                        EndTime = new DateTime(workday.Year, workday.Month, workday.Day, 13, 0, 0)
                    },
                    new Schedule
                    {
                        Employee = employeePair.Item2,
                        StartTime = new DateTime(workday.Year, workday.Month, workday.Day, 13, 0, 0),
                        EndTime = new DateTime(workday.Year, workday.Month, workday.Day, 17, 0, 0)
                    }
                };

        private IEnumerable<(string, string)> GetValidNextRandomPairList(IEnumerable<(string, string)> employeePairs)
        {
            var randomPairs = _people.ToRandomPairs();
            while (InvalidCombo(randomPairs.First(), employeePairs.LastOrDefault()))
            {
                randomPairs = _people.ToRandomPairs();
            }

            return randomPairs;
        }

        private bool InvalidCombo((string, string) current, (string, string) previous)
        {
            var firstPairInCurrentSet = new List<string>
                {
                    current.Item1,
                    current.Item2,
                };

            var lastPairInPreviousSet = new List<string>
                {
                    previous.Item1,
                    previous.Item2
                };

            foreach (var person in firstPairInCurrentSet)
            {
                foreach (var otherPerson in lastPairInPreviousSet)
                {
                    if (person.Equals(otherPerson))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
