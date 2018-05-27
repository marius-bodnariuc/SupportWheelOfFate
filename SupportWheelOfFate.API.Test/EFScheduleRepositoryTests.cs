using Microsoft.EntityFrameworkCore;
using SupportWheelOfFate.API.Persistence;
using SupportWheelOfFate.API.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SupportWheelOfFate.API.Test
{
    public class EFScheduleRepositoryTests
    {
        [Fact]
        public async Task AddSavesToDatabase()
        {
            var dbContextOptions = new DbContextOptionsBuilder<SupportWheelOfFateContext>()
                .UseInMemoryDatabase(databaseName: "TestInMemoryDatabase")
                .Options;

            using (var dbContext = new SupportWheelOfFateContext(dbContextOptions))
            {
                var repo = new EFScheduleRepository(dbContext);
                repo.Add(_testSchedule);
            }

            using (var dbContext = new SupportWheelOfFateContext(dbContextOptions))
            {
                Assert.Equal(1, await dbContext.Schedules.CountAsync());
                Assert.Equal(_testSchedule.Employee, (await dbContext.Schedules.SingleAsync()).Employee);
            }
        }

        #region Private helpers

        private readonly Schedule _testSchedule = new Schedule
        {
            Employee = "John",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(4)
        };

    #endregion Private helpers
}
}
