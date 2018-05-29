using System;
using System.Collections.Generic;
using System.Text;

namespace SupportWheelOfFate.Core
{
    // TODO rename this to MonthlySchedule, maybe?
    public class Schedule
    {
        public string Employee { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public override string ToString()
        {
            return $"[{Employee}: {StartTime} -> {EndTime}]";
        }
    }
}
