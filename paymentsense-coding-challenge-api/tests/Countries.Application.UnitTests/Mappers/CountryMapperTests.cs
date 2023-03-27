using AutoFixture;
using Countries.Application.Mappers;
using Countries.Application.Models;
using FluentAssertions;
using Xunit;

namespace Countries.Application.UnitTests.Mappers
{
    public class CountryMapperTests
    {
        private readonly CountryMapper mapper;
        private readonly Fixture fixture;

        public CountryMapperTests()
        {
            this.mapper = new CountryMapper();
            this.fixture = new Fixture();
        }

        [Fact]
        public void Map_Country()
        {
            var restCountry1 = this.fixture.Create<Infrastructure.Models.RestCountriesModel.Country>();
            var restCountry2 = this.fixture.Create<Infrastructure.Models.RestCountriesModel.Country>();

            restCountry1.Borders = new List<string> { restCountry2.CountryCode };
            restCountry2.Borders = new List<string> { restCountry1.CountryCode };

            var restCountries = new List<Infrastructure.Models.RestCountriesModel.Country> { restCountry1, restCountry2 };

            var country = this.mapper.Map(restCountry1, restCountries);

            country.Name.Should().Be(restCountry1.Name.Common);
            country.FlagUrl.Should().Be(restCountry1.Flags.Png);
            country.Population.Should().Be(restCountry1.Population);

            country.TimeZones.Count().Should().Be(restCountry1.TimeZones.Count());
            country.TimeZones.Select(x => restCountry1.TimeZones.Should().Contain(x));

            country.Currencies.Count().Should().Be(restCountry1.Currencies.Count());
            country.Currencies.Select(x => restCountry1.Currencies.Should().Contain(x));

            country.Languages.Count().Should().Be(restCountry1.Languages.Count());
            country.Languages.Select(x => restCountry1.Languages.Should().Contain(x));

            country.CapitalCities.Count().Should().Be(restCountry1.CapitalCities.Count());
            country.CapitalCities.Select(x => restCountry1.CapitalCities.Should().Contain(x));

            country.Borders.Count().Should().Be(restCountry1.Borders.Count());
            country.Borders.Should().Contain(restCountry2.Name.Common);
        }
    }
}