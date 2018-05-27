using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Repositories
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> GetSchedulesBetween(DateTime startTime, DateTime endTime);
    }
}
