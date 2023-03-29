namespace Countries.Infrastructure.Mappers.Interfaces
{
    public interface ICountryMapper
    {
        Domain.Models.Country Map(Models.RestCountriesModel.Country restCountry, IEnumerable<Models.RestCountriesModel.Country> restCountries);
    }
}
