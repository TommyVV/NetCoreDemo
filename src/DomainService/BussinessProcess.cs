using BussinessInterface;
using Utility;

namespace DomainService
{
    [DependencyInjection(typeof(IBussinessProcess),Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
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
