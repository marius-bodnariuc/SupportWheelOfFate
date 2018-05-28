using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SupportWheelOfFate.Core.Test
{
    public class WorkPatternValidatorTests
    {
        [Fact]
        public void ValidatesWorkPatternAccordingToBusinessRules()
        {
            // Arrange (huge area)
            var mockNoConsecutiveWorkdaysBusinessRule = new Mock<BusinessRule>();
            mockNoConsecutiveWorkdaysBusinessRule.Setup(businessRule => businessRule.CheckAgainst(It.IsAny<WorkPatternRepresentation>()))
                .Returns(NoConsecutiveWorkdaysPredicate);

            var mockAtLeastTwoShiftsInTwoWeeksBusinessRule = new Mock<BusinessRule>();
            mockAtLeastTwoShiftsInTwoWeeksBusinessRule.Setup(businessRule => businessRule.CheckAgainst(It.IsAny<WorkPatternRepresentation>()))
                .Returns(AtLeastTwoShiftsInTwoWeeksPredicate);

            var businessRules = new List<BusinessRule>
            {
                mockNoConsecutiveWorkdaysBusinessRule.Object,
                mockAtLeastTwoShiftsInTwoWeeksBusinessRule.Object
            };

            var validator = new WorkPatternValidator(businessRules);

            // Act and Assert
            Assert.True(validator.Validate(ValidWorkPattern));
            Assert.False(validator.Validate(InvalidWorkPatternWithConsecutiveWorkdays));
            Assert.False(validator.Validate(InvalidWorkPatternWithNoTwoShiftsInTwoWeeks));
            Assert.False(validator.Validate(AnotherInvalidWorkPatternWithNoTwoShiftsInTwoWeeks));

            mockNoConsecutiveWorkdaysBusinessRule.VerifyAll();
            mockAtLeastTwoShiftsInTwoWeeksBusinessRule.VerifyAll();
        }

        #region Private helpers

        private static Func<WorkPatternRepresentation, bool> NoConsecutiveWorkdaysPredicate
        {
            get => (WorkPatternRepresentation representation) => representation.Take(representation.Count() - 1)
                .Zip(representation.Skip(1).Take(representation.Count() - 1), (firstFlag, secondFlag) => new List<int> { firstFlag, secondFlag })
                .Any(pair => pair.All(flag => flag == 1)) == false;
        }

        private static Func<WorkPatternRepresentation, bool> AtLeastTwoShiftsInTwoWeeksPredicate
        {
            get => (WorkPatternRepresentation representation) =>
            {
                for (var startIndex=0; startIndex<representation.Count; startIndex += 10)
                {
                    var twoWeekSequence = representation.Skip(startIndex).Take(10);
                    if (twoWeekSequence.Count(flag => flag == 1) < 2)
                    {
                        return false;
                    }
                }

                return true;
            };
        }

        private static WorkPatternRepresentation AnotherInvalidWorkPatternWithNoTwoShiftsInTwoWeeks
        {
            get => new WorkPatternRepresentation(
                new[] { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
        }

        private static WorkPatternRepresentation InvalidWorkPatternWithNoTwoShiftsInTwoWeeks
        {
            get => new WorkPatternRepresentation(
                new[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        }

        private static WorkPatternRepresentation InvalidWorkPatternWithConsecutiveWorkdays
        {
            get => new WorkPatternRepresentation(
                new[] { 1, 0, 0, 1, 1 });
        }

        private static WorkPatternRepresentation ValidWorkPattern
        {
            get => new WorkPatternRepresentation(
                new[] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 });
        }

        #endregion Private helpers
    }
}
