using System;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Jobs
{
    internal class GenerateSchedulesForNextMonthIfNeeded : IJob
    {
        public GenerateSchedulesForNextMonthIfNeeded()
        {
            // TODO inject repo here
        }

        public async Task Execute()
        {
            // TODO execute if invoked on a day after the month's 23rd
            Console.WriteLine("Generated schedules for next month");
            await Task.CompletedTask;
        }
    }
}