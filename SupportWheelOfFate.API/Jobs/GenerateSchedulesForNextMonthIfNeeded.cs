using SupportWheelOfFate.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Jobs
{
    internal class GenerateSchedulesForNextMonthIfNeededJob : IJob
    {
        private readonly IScheduleRepository _scheduleRepository;

        public GenerateSchedulesForNextMonthIfNeededJob(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task Execute()
        {
            if (DateTime.Now.Day >= 23)
            {
                // simplistic check, but it'll do if no one messes with the DB directly
                if (SchedulesForNextMonthAlreadyExist())
                {
                    Console.WriteLine("Schedules for next month already in place");
                    return;
                }

                var schedules = WorkdaysInMonth.Zip(EmployeePairs, BuildSchedulesForEmployeePair)
                .SelectMany(schedule => schedule);

                // TODO add a bulk insert method, perhaps
                schedules.ToList().ForEach(schedule => _scheduleRepository.Add(schedule));

                Console.WriteLine("Generated schedules for next month");
                await Task.CompletedTask;
            }
            else
            {
                Console.WriteLine("Not generating schedules for next month until the 23rd");
                await Task.CompletedTask;
            }
        }

        private IEnumerable<DateTime> WorkdaysInMonth =>
            Enumerable.Range(1, DaysInMonth)
                .Select(day => new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, day))
                .Where(date => Workdays.Contains(date.DayOfWeek));

        private int DaysInMonth => DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month + 1);

        private IEnumerable<string> Employees =>
            new List<string>
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

        private IEnumerable<DayOfWeek> Workdays =>
            new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };

        private bool SchedulesForNextMonthAlreadyExist()
        {
            var firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
            var schedulesForFirstDayInMonth = _scheduleRepository.GetSchedulesBetween(
                firstDayInMonth, firstDayInMonth.AddDays(1));

            return schedulesForFirstDayInMonth.Any();
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

        private IEnumerable<(string, string)> GetValidNextRandomPairList(IEnumerable<(string, string)> employeePairs)
        {
            var randomPairs = Employees.ToRandomPairs();
            while (InvalidCombo(randomPairs.First(), employeePairs.LastOrDefault()))
            {
                randomPairs = Employees.ToRandomPairs();
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