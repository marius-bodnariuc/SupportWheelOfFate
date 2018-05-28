using System.Collections.Generic;

namespace SupportWheelOfFate.Core
{
    public interface IPatternGenerator
    {
        IEnumerable<IEnumerable<int>> GenerateLazy(int numberOfOptions, int numberOfPositions);
    }
}