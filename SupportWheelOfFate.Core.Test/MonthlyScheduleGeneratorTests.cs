using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SupportWheelOfFate.Core.Test
{
    public class MonthlyScheduleGeneratorTests
    {
        [Fact(Skip = "WIP")]
        public void GeneratesValidSchedulesToCoverAllWorkingDaysInMonth()
        {
            // Arrange
            var generatedWorkPatterns = WorkPatternGenerator
                .GenerateValidPatterns()
                .ToList();

            var monthlyScheduleGenerator = new MonthlyScheduleGenerator(
                People, Shifts, generatedWorkPatterns);

            // Act
            var schedules = monthlyScheduleGenerator.Generate();

            // Assert
            Assert.Equal(People.Count(), schedules.Count());
        }

        #region Private helpers

        private static Func<WorkPatternRepresentation, bool> NoConsecutiveWorkdaysPredicate
        {
            get => (WorkPatternRepresentation representation) => representation.Take(representation.Count() - 1)
                .Zip(representation.Skip(1).Take(representation.Count() - 1), (firstFlag, secondFlag) => new List<int> { firstFlag, secondFlag })
                .Any(pair => pair.All(flag => flag == 1)) == false;
        }

        private static List<BusinessRule> BusinessRules
        {
            get
            {
                var mockBusinessRule = new Mock<BusinessRule>();
                mockBusinessRule.Setup(rule => rule.CheckAgainst(It.IsAny<WorkPatternRepresentation>()))
                    .Returns(NoConsecutiveWorkdaysPredicate);

                var businessRules = new List<BusinessRule>
            {
                mockBusinessRule.Object
            };

                return businessRules;
            }
        }

        private static WorkPatternGenerator WorkPatternGenerator =>
            new WorkPatternGenerator(2, 5,
                new ExhaustiveSearchGenerator(),
                new WorkPatternValidator(BusinessRules));

        private static string[] People =>
            new[]
            {
                "John",
                "Jane",
                "Jack",
                "Jim",
                "Jacqueline",
                "Jenny",
                "Jeroen",
                "Joel",
                "Jojo",
                "Jasmine"
            };

        private static WorkShift[] Shifts =>
            new[]
            {
                WorkShift.Morning,
                WorkShift.Afternoon
            };

        #endregion Private helpers
    }
}
