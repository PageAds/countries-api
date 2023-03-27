using AutoFixture;
using Countries.Application.Mappers;
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
            var restCountry = fixture.Create<Infrastructure.Models.RestCountriesModel.Country>();

            var country = this.mapper.Map(restCountry);

            country.Name.Should().Be(restCountry.Name.Common);
            country.FlagUrl.Should().Be(restCountry.Flags.Png);
        }
    }
}