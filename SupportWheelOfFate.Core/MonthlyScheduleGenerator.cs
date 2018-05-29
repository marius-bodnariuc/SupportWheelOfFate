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

            var monthlyPatterns = CombineWeeklyPatternsIntoMonthlyOnes(selectedWorkPatterns);

            var schedulePatternGenerator = new SchedulePatternGenerator(monthlyPatterns, _shifts);
            var schedulePatterns = schedulePatternGenerator.GenerateAll();

            var schedules = new List<Schedule>();
            for (var personIndex = 0; personIndex < _people.Count(); personIndex += 1)
            {
                for (var workdayIndex = 0; workdayIndex < WorkdaysInMonth.Count(); workdayIndex += 1)
                {
                    var currentDaySchedule = schedulePatterns.ElementAt(personIndex).ElementAt(workdayIndex);
                    if (currentDaySchedule.StartsWith("1"))
                    {
                        var workday = WorkdaysInMonth.ElementAt(workdayIndex);
                        var shift = (WorkShift)Enum.Parse(typeof(WorkShift), currentDaySchedule.Split(",").Skip(1).First());

                        var schedule = new Schedule
                        {
                            Employee = _people.ElementAt(personIndex),
                            StartTime = new DateTime(workday.Year, workday.Month, workday.Day),
                            EndTime = new DateTime(workday.Year, workday.Month, workday.Day)
                        };

                        if (WorkShift.Morning == shift)
                        {
                            schedule.StartTime = schedule.StartTime.AddHours(9);
                        }
                        else
                        {
                            schedule.StartTime = schedule.StartTime.AddHours(13);
                        }

                        schedule.EndTime = schedule.StartTime.AddHours(4);

                        schedules.Add(schedule);
                    }
                }
            }
            
            // NOT GOOD: I'm not making sure the resulting schedules don't include duplicates
            // (e.g. more than 2 schedules on one and the same day)
            return schedules;
        }

        private IEnumerable<WorkPatternRepresentation> CombineWeeklyPatternsIntoMonthlyOnes(
            List<WorkPatternRepresentation> weeklyPatterns)
        {
            DaysInMonth.ToString();
            WorkdaysInMonth.ToString();

            var patternValidator = new WorkPatternValidator(new List<BusinessRule>());
            var monthlyPatterns = new List<WorkPatternRepresentation>();
            var random = new Random();

            foreach (var weeklyPattern in weeklyPatterns)
            {
                var monthlyPattern = weeklyPattern;
                // just consider 5 weekly patterns will cover the month
                // (normally, we should find the first multiple of 5 larger than WorkDaysInMonth)
                // TODO the Times() extension method would be useful here
                for (var count = 0; count < 5; count += 1)
                {
                    var randomIndex = random.Next(0, weeklyPatterns.Count);
                    var randomNextPattern = weeklyPatterns.ElementAt(randomIndex);

                    var candidateForExtendedPattern = new WorkPatternRepresentation(
                        monthlyPattern.Concat(randomNextPattern));

                    while (!patternValidator.Validate(candidateForExtendedPattern))
                    {
                        randomIndex = random.Next(0, weeklyPatterns.Count);
                        randomNextPattern = weeklyPatterns.ElementAt(randomIndex);

                        candidateForExtendedPattern = new WorkPatternRepresentation(
                            monthlyPattern.Concat(randomNextPattern));
                    }

                    // TODO as a safety measure, the above loop should be forcefully
                    // exited if it has been looping for a large number of times

                    monthlyPattern = candidateForExtendedPattern;
                }

                monthlyPatterns.Add(monthlyPattern);
            }

            return monthlyPatterns;
        }

        #region Calendar-related helpers that should probably be moved in a class of their own

        private IEnumerable<DateTime> WorkdaysInMonth =>
            Enumerable.Range(1, DaysInMonth)
                .Select(day => new DateTime(DateTime.Today.Year, DateTime.Today.Month, day))
                .Where(date => Workdays.Contains(date.DayOfWeek));

        private int DaysInMonth => DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

        private IEnumerable<DayOfWeek> Workdays =>
            new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };

        #endregion Calendar-related helpers that should probably be moved in a class of their own
    }
}
