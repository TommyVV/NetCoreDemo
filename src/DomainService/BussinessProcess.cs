using BussinessInterface;
using Utility;

namespace DomainService
{
    [DependencyInjection(typeof(IBussinessProcess))]
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
