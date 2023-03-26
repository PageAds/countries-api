using Countries.Domain.Models;
using Countries.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Country>>> Get()
        {
            return Ok(await this.countriesRepository.Get());
        }
    }
}
