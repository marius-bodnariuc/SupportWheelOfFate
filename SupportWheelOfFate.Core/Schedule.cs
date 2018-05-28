using System;
using System.Collections.Generic;
using System.Text;

namespace SupportWheelOfFate.Core
{
    // TODO rename this to MonthlySchedule, maybe?
    public class Schedule
    {
        public string Person { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public override string ToString()
        {
            return $"[{Person}: {StartTime} -> {EndTime}]";
        }
    }
}
