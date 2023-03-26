using Countries.Application.Models;
using Countries.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Countries.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository countriesRepository;

        public CountriesController(ICountriesRepository countriesRepository)
        {
            this.countriesRepository = countriesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CountriesResponse>> Get()
        {
            var countries = await this.countriesRepository.Get();

            var countriesResponse = new CountriesResponse()
            {
                Countries = countries
            };

            return Ok(countriesResponse);
        }
    }
}
