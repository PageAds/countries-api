using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.IntegrationTests
{
    public class CountriesControllerTests
    {
        private readonly HttpClient client;

        public CountriesControllerTests()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var testServer = new TestServer(builder);
            this.client = testServer.CreateClient();
        }

        [Fact]
        public async Task GetCountries_ReturnsOKResult()
        {
            var response = await this.client.GetAsync("/countries");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
