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
        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<ScheduleDTO>> GetAsync()
        {
            return await Task.FromResult(new List<ScheduleDTO>());
        }
    }
}
