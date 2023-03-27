using Countries.Application.Mappers.Interfaces;

namespace Countries.Application.Mappers
{
    public class CountryMapper : ICountryMapper
    {
        public Domain.Models.Country Map(Infrastructure.Models.RestCountriesModel.Country restCountry)
        {
            return new Domain.Models.Country(
                restCountry.Name.Common,
                restCountry.Flags.Png);
        }
    }
}
