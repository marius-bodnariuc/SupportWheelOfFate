using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SupportWheelOfFate.API.Persistence;

namespace SupportWheelOfFate.API.Repositories
{
    public class EFScheduleRepository : IScheduleRepository
    {
        private readonly SupportWheelOfFateContext _dbContext;

        public EFScheduleRepository(SupportWheelOfFateContext dbContext)
        {
            _dbContext = dbContext;
        }

        // TODO make async
        public IEnumerable<Schedule> GetSchedulesBetween(DateTime startTime, DateTime endTime)
        {
            var schedules = _dbContext.Schedules
                .Where(schedule => schedule.StartTime >= startTime && schedule.EndTime <= endTime)
                .ToList();

            // TODO use ILogger, instead
            Console.WriteLine($"GetSchedulesBetween {startTime.Date} and {endTime.Date}: {schedules.Count()} found");

            return schedules;
        }

        // TODO make async
        public void Add(Schedule schedule)
        {
            _dbContext.Schedules.Add(schedule);
            _dbContext.SaveChanges();

            // TODO use ILogger, instead
            Console.WriteLine($"New schedule added to DB. New total: {_dbContext.Schedules.Count()}");
        }
    }
}
