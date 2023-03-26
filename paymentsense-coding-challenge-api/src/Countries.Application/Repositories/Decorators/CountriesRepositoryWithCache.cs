using Countries.Domain.Models;
using Countries.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Countries.Application.Repositories.Decorators
{
    public class CountriesRepositoryWithCache : ICountriesRepository
    {
        private const string CacheKey = "Countries";

        private readonly ICountriesRepository countriesRepository;
        private readonly IMemoryCache memoryCache;

        public CountriesRepositoryWithCache(
            ICountriesRepository countriesRepository,
            IMemoryCache memoryCache)
        {
            this.countriesRepository = countriesRepository;
            this.memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Country>> Get()
        {
            if (!this.memoryCache.TryGetValue(CacheKey, out IEnumerable<Country> cachedCountries))
            {
                var countries = await this.countriesRepository.Get();

                this.memoryCache.Set(CacheKey, countries, TimeSpan.FromMinutes(60));

                return countries;
            }

            return cachedCountries;
        }
    }
}
