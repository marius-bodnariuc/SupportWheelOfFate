using Microsoft.EntityFrameworkCore;
using SupportWheelOfFate.API.Persistence;
using SupportWheelOfFate.API.Repositories;
using SupportWheelOfFate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Jobs
{
    internal class EnsureSchedulesForCurrentMonthAreInPlaceJob : IJob
    {
        private readonly IScheduleRepository _scheduleRepository;

        public EnsureSchedulesForCurrentMonthAreInPlaceJob(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task Execute()
        {
            // simplistic check, but it'll do if no one messes with the DB directly
            if (SchedulesForCurrentMonthAlreadyExist())
            {
                Console.WriteLine("Schedules for current month already in place");
                return;
            }

            var schedules = new NaiveMonthlyScheduleGenerator(Employees).Generate();

            // TODO add a bulk insert method, perhaps
            schedules.Select(schedule => schedule.ToPersistedModel()).ToList()
                .ForEach(schedule => _scheduleRepository.Add(schedule));

            Console.WriteLine("Generated schedules for current month");
            await Task.CompletedTask;
        }

        private bool SchedulesForCurrentMonthAlreadyExist()
        {
            var firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var schedulesForFirstDayInMonth = _scheduleRepository.GetSchedulesBetween(
                firstDayInMonth, firstDayInMonth.AddDays(1));

            return schedulesForFirstDayInMonth.Any();
        }

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
    }
}