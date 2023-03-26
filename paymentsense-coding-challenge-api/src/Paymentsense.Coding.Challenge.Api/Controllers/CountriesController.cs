using Microsoft.AspNetCore.Mvc;

namespace Paymentsense.Coding.Challenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok();
        }
    }
}
