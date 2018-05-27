using System;
using System.Collections.Generic;

namespace SupportWheelOfFate.API
{
    public class ScheduleDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Employee { get; set; } // TODO turn into an entity
    }
}