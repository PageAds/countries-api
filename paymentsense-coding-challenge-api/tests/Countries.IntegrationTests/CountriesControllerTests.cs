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
        public async Task GetCountries_WhenExternalApiReturnsCountries_ReturnsListOfTheSameCountryNames()
        {
            // Arrange
            var mockedCountries = fixture.CreateMany<Infrastructure.Models.RestCountriesModel.Country>();
            var restCountriesHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockedCountries), new MediaTypeHeaderValue("application/json"))
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
            countriesResponse.Countries.Count().Should().Be(mockedCountries.Count());

            foreach (var country in countriesResponse.Countries)
            {
                mockedCountries.Select(x => x.Name.Common).SingleOrDefault(x => x == country.Name).Should().NotBeNull();
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
            var countriesToReturn = 100;
            var mockedCountries = fixture.CreateMany<Infrastructure.Models.RestCountriesModel.Country>(countriesToReturn);
            var restCountriesHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockedCountries), new MediaTypeHeaderValue("application/json"))
            };

            var client = this.CreateTestHttpClient(restCountriesHttpResponseMessage);

            // Act
            var response = await client.GetAsync($"/countries?pageNumber{pageNumber}&pageSize={pageSize}");

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
