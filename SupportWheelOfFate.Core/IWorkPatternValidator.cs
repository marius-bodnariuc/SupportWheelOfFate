using System;
using System.Collections.Generic;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public interface IWorkPatternValidator
    {
        bool Validate(WorkPatternRepresentation patternRepresentation);
    }
}
