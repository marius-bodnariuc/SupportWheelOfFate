using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportWheelOfFate.Core
{
    public class WorkPatternValidator
    {
        private readonly List<BusinessRule> _businessRules;

        public WorkPatternValidator(List<BusinessRule> businessRules)
        {
            _businessRules = businessRules;
        }

        public bool Validate(WorkPatternRepresentation workPattern)
        {
            // TODO: rewrite this, perhaps by introducing a None() ext method
            return !_businessRules.Any(businessRule =>
                businessRule.CheckAgainst(workPattern) == false);
        }
    }
}
