using Countries.Infrastructure.Mappers.Interfaces;

namespace Countries.Infrastructure.Mappers
{
    public class CountryMapper : ICountryMapper
    {
        public Domain.Models.Country Map(Infrastructure.Models.RestCountriesModel.Country restCountry, IEnumerable<Infrastructure.Models.RestCountriesModel.Country> restCountries)
        {
            return new Domain.Models.Country(
                restCountry.Name.Common,
                restCountry.Flags.Png,
                restCountry.Population,
                restCountry.TimeZones,
                restCountry.Currencies,
                restCountry.Languages,
                restCountry.CapitalCities,
                restCountry.Borders?.Select(border => restCountries.SingleOrDefault(country => country.CountryCode == border)?.Name?.Common));
        }
    }
}
