using BussinessInterface;
using DependencyExtension;
using Microsoft.Extensions.DependencyInjection;

namespace DomainService
{
    [DependencyInjection(typeof(IBussinessProcess),ServiceLifetime.Scoped)]
    public class BussinessProcess:IBussinessProcess
    {
        private readonly IProcess process;
        public BussinessProcess(IProcess process)
        {
            this.process = process;
        }

        public string Process()
        {
            return "hello"+ process.Name;
        }
    }
}
