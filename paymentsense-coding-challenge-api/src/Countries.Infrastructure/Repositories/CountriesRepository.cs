﻿using Countries.Domain.Models;
using Countries.Domain.Repositories.Interfaces;
using Countries.Infrastructure.HttpClients;
using Countries.Infrastructure.Mappers.Interfaces;

namespace Countries.Infrastructure.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly RestCountriesHttpClient restCountriesHttpClient;
        private readonly ICountryMapper countryMapper;

        public CountriesRepository(
            RestCountriesHttpClient restCountriesHttpClient,
            ICountryMapper countryMapper)
        {
            this.restCountriesHttpClient = restCountriesHttpClient;
            this.countryMapper = countryMapper;
        }

        public async Task<IEnumerable<Country>> Get()
        {
            var restCountries = await this.restCountriesHttpClient.Get();

            return restCountries.Select(restCountry => this.countryMapper.Map(restCountry, restCountries));
        }
    }
}
