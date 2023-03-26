using Microsoft.AspNetCore.Mvc;

namespace Countries.Controllers
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
