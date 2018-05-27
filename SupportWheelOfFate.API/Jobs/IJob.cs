using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Jobs
{
    internal interface IJob
    {
        Task Execute();
    }
}