using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class SchedulePatternGenerator
    {
        private readonly IEnumerable<WorkPatternRepresentation> _workPatterns;
        private readonly IEnumerable<WorkShift> _shifts;

        public SchedulePatternGenerator(
            IEnumerable<WorkPatternRepresentation> workPatterns,
            IEnumerable<WorkShift> shifts)
        {
            _workPatterns = workPatterns;
            _shifts = shifts;
        }

        public IEnumerable<SchedulePatternRepresentation> GenerateAll()
        {
            var schedulePatterns = _workPatterns.Select(workPattern =>
                _shifts.Select(shift =>
                    new SchedulePatternRepresentation(workPattern.Select(workPatternDayFlag =>
                        BuildDaySchedulePatternRepresentation(workPatternDayFlag, shift)))))
                .SelectMany(schedule => schedule)
                .ToList();

            return schedulePatterns;
        }

        private string BuildDaySchedulePatternRepresentation(
            int workPatternFlag, WorkShift shift)
        {
            var daySchedule = $"{workPatternFlag}";
            if (workPatternFlag == 1)
            {
                daySchedule += $",{shift}";
            }

            return daySchedule;
        }
    }
}
