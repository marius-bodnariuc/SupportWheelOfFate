using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Repositories
{
    public class DummyEmployeeRepository : IEmployeeRepository
    {
        public IEnumerable<string> GetAll()
        {
            return new List<string>
            {
                "John",
                "Jane",
                "Jack",
                "Jim",
                "Jacqueline",
                "Jenny",
                "Jeroen",
                "Joel",
                "Jojo",
                "Jasmine"
            };
        }
    }
}
