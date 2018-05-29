﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API
{
    public static class Extensions
    {
        public static ScheduleDTO ToDTO(this Schedule schedule)
        {
            return new ScheduleDTO
            {
                Employee = schedule.Employee,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime
            };
        }
    }
}
