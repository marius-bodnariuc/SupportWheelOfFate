using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Repositories
{
    public class DummyScheduleRepository : IScheduleRepository
    {
        public void Add(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Schedule> GetSchedulesBetween(DateTime startTime, DateTime endTime)
        {
            return new List<Schedule>
            {
                new Schedule
                {
                    Id = 1,
                    Employee = "Jimmy",
                    StartTime = new DateTime(2018, 5, 25, 9, 0, 0),
                    EndTime = new DateTime(2018, 5, 25, 13, 0, 0),
                },
                new Schedule
                {
                    Id = 2,
                    Employee = "Jack",
                    StartTime = new DateTime(2018, 5, 25, 13, 0, 0),
                    EndTime = new DateTime(2018, 5, 25, 17, 0, 0),
                },
                new Schedule
                {
                    Id = 3,
                    Employee = "Jane",
                    StartTime = new DateTime(2018, 5, 26, 9, 0, 0),
                    EndTime = new DateTime(2018, 5, 26, 13, 0, 0),
                },
                new Schedule
                {
                    Id = 4,
                    Employee = "John",
                    StartTime = new DateTime(2018, 5, 26, 13, 0, 0),
                    EndTime = new DateTime(2018, 5, 26, 17, 0, 0),
                },
            };
        }
    }
}
