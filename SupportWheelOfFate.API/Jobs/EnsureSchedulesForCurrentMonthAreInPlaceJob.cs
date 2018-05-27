using Microsoft.EntityFrameworkCore;
using SupportWheelOfFate.API.Persistence;
using SupportWheelOfFate.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Jobs
{
    internal class EnsureSchedulesForCurrentMonthAreInPlaceJob
    {
        private readonly IScheduleRepository _scheduleRepository;

        public EnsureSchedulesForCurrentMonthAreInPlaceJob(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task Execute()
        {
            // TODO move region to its own method
            var firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var schedulesForFirstDayInMonth = _scheduleRepository.GetSchedulesBetween(
                firstDayInMonth, firstDayInMonth.AddDays(1));

            // simplistic check, but it'll do if no one messes with the DB directly
            if (schedulesForFirstDayInMonth.Any())
            {
                Console.WriteLine("Schedules for current month already in place");
                return;
            }

            // TODO move region to its own method
            var now = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var workdaysInMonth = Enumerable.Range(1, daysInMonth)
                .Select(day => new DateTime(now.Year, now.Month, day))
                .Where(date => Workdays.Contains(date.DayOfWeek));

            var employeePairs = Enumerable.Empty<(string, string)>();
            (daysInMonth / Workdays.Count() + 1).Times(() =>
            {
                // TODO: make sure the first pair in a new set doesn't include
                // any of the employees from the last pair in previous set
                employeePairs = employeePairs.Concat(Employees.ToRandomPairs());
            });

            var schedules = workdaysInMonth.Zip(employeePairs,
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
                }).SelectMany(schedule => schedule);

            schedules.ToList().ForEach(schedule => _scheduleRepository.Add(schedule));

            Console.WriteLine("Generated schedules for current month");
            await Task.CompletedTask;
        }

        private IEnumerable<string> Employees
        {
            get => new List<string>
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
        }

        private IEnumerable<DayOfWeek> Workdays
        {
            get => new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };
        }
    }
}