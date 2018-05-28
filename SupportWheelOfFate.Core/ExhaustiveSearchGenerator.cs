using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class ExhaustiveSearchGenerator
    {
        private int _numberOfOptions;

        public ExhaustiveSearchGenerator(int numberOfOptions)
        {
            _numberOfOptions = numberOfOptions;
        }

        public IEnumerable<IEnumerable<int>> GenerateLazy(int numberOfPositions)
        {
            if (numberOfPositions <= 0)
            {
                yield return Enumerable.Empty<int>();
            }
            else
            {
                for (var option = 0; option < _numberOfOptions; option++)
                {
                    foreach (var combo in GenerateLazy(numberOfPositions - 1))
                    {
                        yield return new[] { option }.Concat(combo);
                    }
                }
            }
        }
    }
}
