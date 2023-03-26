namespace Countries.Application.Mappers.Interfaces
{
    public interface ICountryMapper
    {
        Domain.Models.Country Map(Infrastructure.Models.RestCountriesModel.Country restCountry);
    }
}
