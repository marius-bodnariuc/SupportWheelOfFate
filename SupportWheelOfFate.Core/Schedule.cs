using System;
using System.Collections.Generic;
using System.Text;

namespace SupportWheelOfFate.Core
{
    // TODO rename this to MonthlySchedule, maybe?
    public class Schedule
    {
        public string Person { get; internal set; }
        public SchedulePatternRepresentation SchedulePattern { get; internal set; }

        public override string ToString()
        {
            return $"[{Person}: {SchedulePattern.ToString()}]";
        }
    }
}
