using AutoFixture;
using Countries.Api;
using Countries.Domain.Models;
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
        public async Task GetCountries_ReturnsOKResult()
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
        public async Task GetCountries_WhenRepositoryReturnsCountries_ReturnsListOfTheSameCountryNames()
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
            var responseString = await response.Content.ReadAsStringAsync();

            var countriesResponse = JsonConvert.DeserializeObject<IEnumerable<Country>>(responseString);
            countriesResponse.Should().NotBeNull();
            countriesResponse.Count().Should().Be(countries.Count());
            foreach (var country in countries)
            {
                countriesResponse.SingleOrDefault(x => x.Name == country.Name.Common).Should().NotBeNull();
            }
        }

        public HttpClient CreateTestHttpClient(HttpResponseMessage restCountriesHttpResponseMessage)
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
                BaseAddress = new Uri("https://test.com")
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
