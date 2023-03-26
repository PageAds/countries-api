using Countries.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Countries.IntegrationTests
{
    public class HealthcheckTests
    {
        private readonly HttpClient _client;

        public HealthcheckTests()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var testServer = new TestServer(builder);
            _client = testServer.CreateClient();
        }

        [Fact]
        public async Task Health_OnInvoke_ReturnsHealthy()
        {
            var response = await _client.GetAsync("/health");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseString.Should().Be("Healthy");
        }
    }
}
