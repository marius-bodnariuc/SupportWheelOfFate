using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class WorkPatternGenerator
    {
        private readonly int _numberOfOptions;
        private readonly int _numberOfPositions;

        private readonly IPatternGenerator _patternGenerator;
        private readonly IWorkPatternValidator _patternValidator;

        public WorkPatternGenerator(int numberOfOptions, int numberOfPositions,
            IPatternGenerator patternGenerator,
            IWorkPatternValidator patternValidator)
        {
            _numberOfOptions = numberOfOptions;
            _numberOfPositions = numberOfPositions;
            _patternGenerator = patternGenerator;
            _patternValidator = patternValidator;
        }

        public IEnumerable<WorkPatternRepresentation> GenerateValidPatterns()
        {
            return _patternGenerator.GenerateLazy(_numberOfOptions, _numberOfPositions)
                .Select(pattern => new WorkPatternRepresentation(pattern))
                .Where(patternRepresentation => _patternValidator.Validate(patternRepresentation))
                .ToList();
        }
    }
}
