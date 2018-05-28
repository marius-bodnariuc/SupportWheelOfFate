using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class ExhaustiveSearchGenerator : IPatternGenerator
    {
        public IEnumerable<IEnumerable<int>> GenerateLazy(int numberOfOptions, int numberOfPositions)
        {
            if (numberOfPositions <= 0)
            {
                yield return Enumerable.Empty<int>();
            }
            else
            {
                for (var option = 0; option < numberOfOptions; option++)
                {
                    foreach (var combo in GenerateLazy(numberOfOptions, numberOfPositions - 1))
                    {
                        yield return new[] { option }.Concat(combo);
                    }
                }
            }
        }
    }
}
