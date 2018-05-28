using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SupportWheelOfFate.Core.Test
{
    public class SchedulePatternGeneratorTests
    {
        [Fact]
        public void GeneratesSchedulesByDoingACrossProductOnWorkPatternsAndShifts()
        {
            var shifts = new[] { WorkShift.Morning, WorkShift.Afternoon };
            var workPatterns = new[]
            {
                new [] { 0, 1, 0, 1, 0 },
                new [] { 1, 0, 1, 0, 1 }
            }.Select(pattern => new WorkPatternRepresentation(pattern));

            var schedulePatternGenerator = new SchedulePatternGenerator(workPatterns, shifts);

            var schedulePatterns = schedulePatternGenerator.GenerateAll();

            Assert.Equal(shifts.Count() * workPatterns.Count(), schedulePatterns.Count());
        }
    }
}
