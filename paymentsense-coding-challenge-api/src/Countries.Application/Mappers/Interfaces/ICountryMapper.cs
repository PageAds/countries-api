namespace Countries.Application.Mappers.Interfaces
{
    public interface ICountryMapper
    {
        Domain.Models.Country Map(Infrastructure.Models.RestCountriesModel.Country restCountry, IEnumerable<Infrastructure.Models.RestCountriesModel.Country> restCountries);
    }
}
