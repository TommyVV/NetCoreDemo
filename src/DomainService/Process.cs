using BussinessInterface;
using Utility;

namespace DomainService
{
    [DependencyInjection(typeof(IProcess))]
    public class Process: IProcess
    {
        public string Name => "Tommy IOC";
    }
}
