using System;
using System.Collections.Generic;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public abstract class BusinessRule
    {
        public abstract bool CheckAgainst(WorkPatternRepresentation workPattern);
    }
}
