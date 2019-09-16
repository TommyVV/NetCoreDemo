using BussinessInterface;
using Microsoft.AspNetCore.Mvc;

namespace ApiService
{
    public class TestController:Controller
    {
        private readonly IBussinessProcess bussiness;

        public TestController(IBussinessProcess bussiness)
        {
            this.bussiness = bussiness;
        }

        [Route("api/test")]
        [HttpGet]
        public string Test()
        {
            return bussiness.Process();
        }
    }
}
