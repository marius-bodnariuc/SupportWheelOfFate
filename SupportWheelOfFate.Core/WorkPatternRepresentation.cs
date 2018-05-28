using System;
using System.Collections.Generic;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class WorkPatternRepresentation : List<int>
    {
        public WorkPatternRepresentation(IEnumerable<int> collection) : base(collection)
        {
        }
    }
}
