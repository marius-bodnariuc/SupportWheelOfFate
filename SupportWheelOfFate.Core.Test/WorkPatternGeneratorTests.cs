using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SupportWheelOfFate.Core.Test
{
    public class WorkPatternGeneratorTests
    {
        [Fact]
        public void GeneratesValidWorkPatterns()
        {
            // Arrange (huge area)
            var numberOfOptions = 2;
            var numberOfPositions = 5;

            var mockPatternGenerator = new Mock<IPatternGenerator>();
            mockPatternGenerator.Setup(generator => generator.GenerateLazy(numberOfOptions, numberOfPositions))
                .Returns(PatternsToBeGenerated);

            var mockWorkPatternValidator = new Mock<IWorkPatternValidator>();
            mockWorkPatternValidator.Setup(validator => validator.Validate(It.IsAny<WorkPatternRepresentation>()))
                .Returns(NoConsecutiveWorkdaysPredicate);

            var workPatternGenerator = new WorkPatternGenerator(
                numberOfOptions, numberOfPositions,
                mockPatternGenerator.Object,
                mockWorkPatternValidator.Object);

            // Act
            var validWorkPatterns = workPatternGenerator.GenerateValidPatterns();

            // Assert
            Assert.Equal(2, validWorkPatterns.Count());
            Assert.Collection(validWorkPatterns,
                firstPattern => Assert.Equal(PatternsToBeGenerated.First(), firstPattern),
                secondPattern => Assert.Equal(PatternsToBeGenerated.Skip(2).First(), secondPattern));

            mockPatternGenerator.VerifyAll();
            mockWorkPatternValidator.VerifyAll();
        }

        #region Private helpers

        private static List<IEnumerable<int>> PatternsToBeGenerated
        {
            get => new List<IEnumerable<int>>
            {
                new List<int> { 1, 0, 1, 0, 1 },
                new List<int> { 0, 1, 1, 0, 1 },
                new List<int> { 0, 1, 0, 1, 0 },
            };
        }

        private static Func<WorkPatternRepresentation, bool> NoConsecutiveWorkdaysPredicate
        {
            get => (WorkPatternRepresentation representation) => representation.Take(representation.Count() - 1)
                .Zip(representation.Skip(1).Take(representation.Count() - 1), (firstFlag, secondFlag) => new List<int> { firstFlag, secondFlag })
                .Any(pair => pair.All(flag => flag == 1)) == false;
        }

        #endregion Private helpers
    }
}
