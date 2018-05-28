using System;
using System.Collections.Generic;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class SchedulePatternRepresentation : List<string>
    {
        public SchedulePatternRepresentation(IEnumerable<string> collection)
            : base(collection)
        {

        }
    }
}
