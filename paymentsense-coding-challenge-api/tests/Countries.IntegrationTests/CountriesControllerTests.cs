using AutoFixture;
using Countries.Api;
using Countries.Application.Models;
using Countries.Infrastructure.HttpClients;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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
        public async Task GetCountries_ReturnsOKResponse()
        {
            // Arrange
            var countries = fixture.CreateMany<Infrastructure.Models.RestCountriesModel.Country>();
            var restCountriesHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(countries), new MediaTypeHeaderValue("application/json"))
            };

            var client = this.CreateTestHttpClient(restCountriesHttpResponseMessage);

            // Act
            var response = await client.GetAsync("/countries");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetCountries_WhenExternalApiReturnsCountries_ReturnsListOfCountries()
        {
            // Arrange
            var mockCountry1 = this.fixture.Create<Infrastructure.Models.RestCountriesModel.Country>();
            var mockCountry2 = this.fixture.Create<Infrastructure.Models.RestCountriesModel.Country>();
            var mockCountry3 = this.fixture.Create<Infrastructure.Models.RestCountriesModel.Country>();

            mockCountry1.Borders = new List<string> { mockCountry2.CountryCode };
            mockCountry2.Borders = new List<string> { mockCountry3.CountryCode };
            mockCountry3.Borders = new List<string> { mockCountry1.CountryCode };

            var mockCountries = new List<Infrastructure.Models.RestCountriesModel.Country> { mockCountry1, mockCountry2, mockCountry3 };

            var restCountriesHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockCountries), new MediaTypeHeaderValue("application/json"))
            };

            var client = this.CreateTestHttpClient(restCountriesHttpResponseMessage);

            // Act
            var response = await client.GetAsync("/countries");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();

            var countriesResponse = JsonConvert.DeserializeObject<CountriesResponse>(responseString);
            countriesResponse.Should().NotBeNull();
            countriesResponse.Countries.Should().NotBeNull();
            countriesResponse.Countries.Count().Should().Be(mockCountries.Count());

            foreach (var country in countriesResponse.Countries)
            {
                var mockedCountry = mockCountries.Single(x => x.Name.Common == country.Name);
                country.Name.Should().Be(mockedCountry.Name.Common);
                country.FlagUrl.Should().Be(mockedCountry.Flags.Png);
                country.Population.Should().Be(mockedCountry.Population);

                country.TimeZones.Count().Should().Be(mockedCountry.TimeZones.Count());
                country.TimeZones.Select(x => mockedCountry.TimeZones.Should().Contain(x));

                country.Currencies.Count().Should().Be(mockedCountry.Currencies.Count());
                country.Currencies.Select(x => mockedCountry.Currencies.Should().Contain(x));

                country.Languages.Count().Should().Be(mockedCountry.Languages.Count());
                country.Languages.Select(x => mockedCountry.Languages.Should().Contain(x));

                country.CapitalCities.Count().Should().Be(mockedCountry.CapitalCities.Count());
                country.CapitalCities.Select(x => mockedCountry.CapitalCities.Should().Contain(x));

                country.Borders.Count().Should().Be(mockedCountry.Borders.Count());

                foreach (var mockedBorder in mockedCountry.Borders)
                {
                    var mockedBorderedCountry = mockCountries.Single(x => x.CountryCode == mockedBorder);
                    country.Borders.Should().Contain(mockedBorderedCountry.Name.Common);
                }
            }
        }

        [Fact]
        public async Task GetCountries_WhenSubsequentRequestsAreMade_RequestToRESTCountriesIsOnlyPerformedOnce()
        {
            // Arrange
            var mockedCountries = fixture.CreateMany<Infrastructure.Models.RestCountriesModel.Country>();
            var restCountriesHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockedCountries), new MediaTypeHeaderValue("application/json"))
            };

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(restCountriesHttpResponseMessage);

            var restCountriesHttpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = fixture.Create<Uri>()
            };

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<RestCountriesHttpClient>((sp) => { return new RestCountriesHttpClient(restCountriesHttpClient); });
                    });
                });

            var client = application.CreateClient();

            // Act
            await client.GetAsync("/countries");
            await client.GetAsync("/countries");
            await client.GetAsync("/countries");

            // Assert
            httpMessageHandlerMock.Protected().Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Theory]
        [InlineData(1, 5, 100)]
        [InlineData(2, 10, 200)]
        [InlineData(3, 20, 500)]
        public async Task GetCountries_ReturnsPaginatedResponse(int pageNumber, int pageSize, int totalRecords)
        {
            // Arrange
            var countriesToReturn = totalRecords;
            var mockedCountries = fixture.CreateMany<Infrastructure.Models.RestCountriesModel.Country>(countriesToReturn);
            var restCountriesHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockedCountries), new MediaTypeHeaderValue("application/json"))
            };

            var client = this.CreateTestHttpClient(restCountriesHttpResponseMessage);

            // Act
            var response = await client.GetAsync($"/countries?pageNumber={pageNumber}&pageSize={pageSize}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();

            var countriesResponse = JsonConvert.DeserializeObject<CountriesResponse>(responseString);
            countriesResponse.Should().NotBeNull();
            countriesResponse.PageNumber.Should().Be(pageNumber);
            countriesResponse.PageSize.Should().Be(pageSize);
            countriesResponse.TotalRecords.Should().Be(totalRecords);
            countriesResponse.TotalPages.Should().Be(totalRecords / pageSize);
            countriesResponse.Countries.Should().NotBeNull();
            countriesResponse.Countries.Count().Should().Be(pageSize);
            
            foreach (var country in countriesResponse.Countries)
            {
                mockedCountries.Select(x => x.Name.Common).Should().Contain(country.Name);
            }
        }

        private HttpClient CreateTestHttpClient(HttpResponseMessage restCountriesHttpResponseMessage)
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(restCountriesHttpResponseMessage);

            var restCountriesHttpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = fixture.Create<Uri>()
            };

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<RestCountriesHttpClient>((sp) => { return new RestCountriesHttpClient(restCountriesHttpClient); });
                    });
                });

            return application.CreateClient();
        }
    }
}
