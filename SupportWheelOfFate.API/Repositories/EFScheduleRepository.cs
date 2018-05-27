using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        // TODO make async
        public void Add(Schedule schedule)
        {
            _dbContext.Schedules.Add(schedule);
            _dbContext.SaveChanges();
        }
    }
}
