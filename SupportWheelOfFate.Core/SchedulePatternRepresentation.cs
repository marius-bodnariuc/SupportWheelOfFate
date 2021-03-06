﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class SchedulePatternRepresentation : List<string>
    {
        public SchedulePatternRepresentation(IEnumerable<string> collection)
            : base(collection)
        {

        }

        public override string ToString()
        {
            return string.Join(",", this.Select(str => $"{{{str}}}"));
        }
    }
}
