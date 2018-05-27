using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SupportWheelOfFate.API.Controllers
{
    [Route("api/[controller]")]
    public class SchedulesController : Controller
    {
        private readonly List<ScheduleDTO> DummyDailyWorkSchedulesCollection = new List<ScheduleDTO>
        {
            new ScheduleDTO
            {
                Employee = "Jimmy",
                StartTime = new DateTime(2018, 5, 25, 9, 0, 0),
                EndTime = new DateTime(2018, 5, 25, 13, 0, 0),
            },
            new ScheduleDTO
            {
                Employee = "Jack",
                StartTime = new DateTime(2018, 5, 25, 13, 0, 0),
                EndTime = new DateTime(2018, 5, 25, 17, 0, 0),
            },
            new ScheduleDTO
            {
                Employee = "Jane",
                StartTime = new DateTime(2018, 5, 26, 9, 0, 0),
                EndTime = new DateTime(2018, 5, 26, 13, 0, 0),
            },
            new ScheduleDTO
            {
                Employee = "John",
                StartTime = new DateTime(2018, 5, 26, 13, 0, 0),
                EndTime = new DateTime(2018, 5, 26, 17, 0, 0),
            },
        };

        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<ScheduleDTO>> GetAsync()
        {
            return await Task.FromResult(DummyDailyWorkSchedulesCollection);
        }
    }
}
