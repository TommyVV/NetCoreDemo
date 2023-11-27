using BussinessInterface;
using DependencyExtension;

namespace DomainService
{
    [DependencyInjection(typeof(IProcess))]
    public class Process: IProcess
    {
        public string Name => "Tommy IOC";
    }
}
