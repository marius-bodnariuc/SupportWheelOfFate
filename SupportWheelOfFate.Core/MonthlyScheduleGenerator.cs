using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SupportWheelOfFate.Core
{
    public class MonthlyScheduleGenerator
    {
        private readonly IEnumerable<string> _people;
        private readonly IEnumerable<WorkShift> _shifts;
        private readonly IEnumerable<WorkPatternRepresentation> _validWorkPatternsToChooseFrom;

        public MonthlyScheduleGenerator(IEnumerable<string> people, IEnumerable<WorkShift> shifts,
            IEnumerable<WorkPatternRepresentation> validWorkPatternsToChooseFrom)
        {
            _validWorkPatternsToChooseFrom = validWorkPatternsToChooseFrom;
            _shifts = shifts;
            _people = people;
        }

        public IEnumerable<Schedule> Generate()
        {
            var selectedWorkPatterns = _validWorkPatternsToChooseFrom
                .Where(workPattern => workPattern.Count(flag => flag == 1) == 1)
                .Select(workPattern => new WorkPatternRepresentation(workPattern))
                .ToList();

            var schedulePatternGenerator = new SchedulePatternGenerator(selectedWorkPatterns, _shifts);
            var schedulePatterns = schedulePatternGenerator.GenerateAll();

            var schedules = _people.Zip(schedulePatterns,
                (person, schedulePattern) => new Schedule
                {
                    Person = person,
                    SchedulePattern = schedulePattern
                });

            return schedules;
        }
    }
}
