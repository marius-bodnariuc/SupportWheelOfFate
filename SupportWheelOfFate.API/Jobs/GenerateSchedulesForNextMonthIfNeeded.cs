using SupportWheelOfFate.API.Repositories;
using SupportWheelOfFate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Jobs
{
    internal class GenerateSchedulesForNextMonthIfNeededJob : IJob
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public GenerateSchedulesForNextMonthIfNeededJob(
            IScheduleRepository scheduleRepository,
            IEmployeeRepository employeeRepository)
        {
            _scheduleRepository = scheduleRepository;
            _employeeRepository = employeeRepository;
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

                // TODO make sure the schedules for the current month are in place;
                // right now, this is at the mercy of Hanfire's scheduler ...
                var schedules = new NaiveMonthlyScheduleGenerator(Employees)
                    .Generate(forNextMonth: true);

                // TODO add a bulk insert method, perhaps
                schedules.Select(schedule => schedule.ToPersistedModel()).ToList()
                    .ForEach(schedule => _scheduleRepository.Add(schedule));

                Console.WriteLine("Generated schedules for next month");
                await Task.CompletedTask;
            }
            else
            {
                Console.WriteLine("Not generating schedules for next month until the 23rd");
                await Task.CompletedTask;
            }
        }

        private bool SchedulesForNextMonthAlreadyExist()
        {
            var firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
            var schedulesForFirstDayInMonth = _scheduleRepository.GetSchedulesBetween(
                firstDayInMonth, firstDayInMonth.AddDays(1));

            return schedulesForFirstDayInMonth.Any();
        }

        private IEnumerable<string> Employees =>
            _employeeRepository.GetAll();
    }
}