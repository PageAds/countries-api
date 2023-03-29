using Countries.Api.Filters;
using Countries.Api.Models;
using Countries.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        public async Task<ActionResult<CountriesResponse>> Get([FromQuery] PaginationFilter filter)
        {
            var paginationFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var countries = await this.countriesRepository.Get();

            var countriesPaginated = countries
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);

            var countriesResponse = new CountriesResponse()
            {
                PageNumber = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalRecords = countries.Count(),
                TotalPages = (int)Math.Ceiling((double)countries.Count() / (double)paginationFilter.PageSize),
                Countries = countriesPaginated
            };

            return Ok(countriesResponse);
        }
    }
}
