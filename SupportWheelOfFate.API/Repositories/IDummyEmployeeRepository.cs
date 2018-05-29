using System.Collections.Generic;

namespace SupportWheelOfFate.API.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<string> GetAll();
    }
}