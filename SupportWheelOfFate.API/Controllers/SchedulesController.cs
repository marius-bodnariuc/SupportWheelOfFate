﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportWheelOfFate.API.Repositories;

namespace SupportWheelOfFate.API.Controllers
{
    [Route("api/[controller]")]
    public class SchedulesController : Controller
    {
        private IScheduleRepository _scheduleRepository;

        public SchedulesController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        // GET api/<controller>
        // TODO take care not to return too much data here
        [HttpGet]
        public async Task<IEnumerable<ScheduleDTO>> GetAsync(
            [FromQuery]DateTime? startTime = null,
            [FromQuery]DateTime? endTime = null)
        {
            if (!startTime.HasValue) { startTime = DateTime.Now.AddDays(-Constants.DefaultIntervalInDays); }
            if (!endTime.HasValue)   { endTime = DateTime.Now.AddDays(Constants.DefaultIntervalInDays); }

            var schedules = _scheduleRepository.GetSchedulesBetween(startTime.Value, endTime.Value);
            var scheduleDTOs = schedules.Select(schedule => schedule.ToDTO());

            return await Task.FromResult(scheduleDTOs);
        }
    }
}
