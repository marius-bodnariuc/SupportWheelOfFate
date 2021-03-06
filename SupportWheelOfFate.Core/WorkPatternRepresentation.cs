﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SupportWheelOfFate.Core
{
    public class WorkPatternRepresentation : List<int>
    {
        public WorkPatternRepresentation(IEnumerable<int> collection)
            : base(collection)
        {
        }

        public override string ToString()
        {
            return string.Join(",", this.Select(num => num.ToString()));
        }
    }
}
