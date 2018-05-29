using SupportWheelOfFate.API.Repositories;
using System;
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
                // TODO execute if invoked on a day after the month's 23rd
                Console.WriteLine("Generated schedules for next month");
                await Task.CompletedTask;
            }
            else
            {
                Console.WriteLine("Not generating schedules for next month until the 23rd");
                await Task.CompletedTask;
            }
        }
    }
}