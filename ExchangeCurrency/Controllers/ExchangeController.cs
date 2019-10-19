using Microsoft.AspNetCore.Mvc;

namespace ExchangeCurrency.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "value1";
        }
    }
}