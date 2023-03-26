using AutoFixture;
using Countries.Api;
using Countries.Domain.Models;
using Countries.Domain.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Countries.IntegrationTests
{
    public class CountriesControllerTests
    {
        private readonly Fixture fixture;

        public CountriesControllerTests()
        {
            this.fixture = new Fixture();
        }

        [Fact]
        public async Task GetCountries_ReturnsOKResult()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<ICountriesRepository>((sp) => { return new Mock<ICountriesRepository>().Object; });
                    });
                });

            // Act
            var client = application.CreateClient();

            // Assert
            var response = await client.GetAsync("/countries");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetCountries_WhenRepositoryReturnsCountries_ReturnsListOfTheSameCountryNames()
        {
            // Arrange
            var countriesRepositoryMock = new Mock<ICountriesRepository>();

            var countries = fixture.CreateMany<Country>();

            countriesRepositoryMock.Setup(x => x.Get()).ReturnsAsync(countries);

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<ICountriesRepository>((sp) => { return countriesRepositoryMock.Object; });
                    });
                });

            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/countries");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();

            var countriesResponse = JsonConvert.DeserializeObject<IEnumerable<Country>>(responseString);
            countriesResponse.Should().NotBeNull();
            countriesResponse.Count().Should().Be(countries.Count());
            foreach (var country in countries)
            {
                countriesResponse.SingleOrDefault(x => x.Name == country.Name).Should().NotBeNull();
            }
        }
    }
}
