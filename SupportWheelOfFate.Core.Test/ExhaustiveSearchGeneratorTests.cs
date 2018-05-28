using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SupportWheelOfFate.Core.Test
{
    public class ExhaustiveSearchGeneratorTests
    {
        [Fact]
        public void GeneratesTwoUniqueCombinationsForTwoOptionsAndOnePosition()
        {
            var combinations = GenerateAllCombinations(numberOfOptions: 2, numberOfPositions: 1);

            Assert.Equal(2, combinations.Count());
            Assert.Collection(combinations,
                firstCombination => Assert.Equal(new List<int> { 0 }, firstCombination),
                secondCombination => Assert.Equal(new List<int> { 1 }, secondCombination));
        }

        [Fact]
        public void GeneratesThirtyTwoUniqueCombinationsForTwoOptionsAndFivePositions()
        {
            var combinations = GenerateAllCombinations(numberOfOptions: 2, numberOfPositions: 5);

            Assert.Equal(32, combinations.Count());
            Assert.Equal(combinations.ToHashSet().ToList(), combinations.ToList());
            new List<IEnumerable<int>>
            {
                new List<int> { 0, 0, 0, 0, 0 },
                new List<int> { 0, 1, 0, 1, 0 },
                new List<int> { 1, 1, 0, 1, 1 },
                new List<int> { 1, 1, 1, 1, 1 },
            }.ForEach(combination => Assert.Contains(combination, combinations));
        }

        [Fact]
        public void GeneratesEightyOneUniqueCombinationsForThreeOptionsAndFourPositions()
        {
            var combinations = GenerateAllCombinations(numberOfOptions: 3, numberOfPositions: 4);

            Assert.Equal(81, combinations.Count());
            Assert.Equal(combinations.ToHashSet().ToList(), combinations.ToList());
        }

        [Fact]
        public void GeneratesZeroCombinationsForLessThanOneOption()
        {
            new List<int> { 0, -3 }.ForEach(options =>
            {
                var combinations = GenerateAllCombinations(numberOfOptions: options, numberOfPositions: 3);
                Assert.Empty(combinations);
            });
        }

        [Fact]
        public void GeneratesOneEmptyCombinationForLessThanOnePosition()
        {
            new List<int> { 0, -3 }.ForEach(positions =>
            {
                var combinations = GenerateAllCombinations(numberOfOptions: 2, numberOfPositions: positions);

                Assert.Single(combinations);
                Assert.Collection(combinations,
                    combination => Assert.Empty(combination));
            });
        }

        private IEnumerable<IEnumerable<int>> GenerateAllCombinations(int numberOfOptions, int numberOfPositions)
        {
            var generator = new ExhaustiveSearchGenerator();
            return generator.GenerateLazy(numberOfOptions, numberOfPositions);
        }
    }
}
