using Moq;
using Newtonsoft.Json;
using SupportWheelOfFate.API.Controllers;
using SupportWheelOfFate.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SupportWheelOfFate.API.Test
{
    public class SchedulesControllerTests
    {
        [Fact]
        public async Task SchedulesGet_ReturnsACollectionOfScheduleDTOs()
        {
            var mockRepo = new Mock<IScheduleRepository>();
            mockRepo.Setup(repo => repo.GetSchedulesBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(DummyDailyWorkSchedulesCollection);

            var controller = new SchedulesController(mockRepo.Object);

            var scheduleDTOs = await controller.GetAsync();
            
            Assert.Equal(
                DummyDailyWorkSchedulesCollection.Select(schedule => (schedule.Employee, schedule.StartTime)),
                scheduleDTOs.Select(scheduleDTO => (scheduleDTO.Employee, scheduleDTO.StartTime)));

            mockRepo.Verify(repo => repo.GetSchedulesBetween(
                It.Is<DateTime>(dateTime => dateTime.Date == DateTime.Now.AddDays(-Constants.DefaultIntervalInDays).Date),
                It.Is<DateTime>(dateTime => dateTime.Date == DateTime.Now.AddDays(Constants.DefaultIntervalInDays).Date)));
        }

        #region Private helpers

        private readonly List<Schedule> DummyDailyWorkSchedulesCollection = new List<Schedule>
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

        #endregion Private helpers
    }
}
