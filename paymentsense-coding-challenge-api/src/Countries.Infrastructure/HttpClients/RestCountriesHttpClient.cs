using Countries.Infrastructure.Models.RestCountriesModel;

namespace Countries.Infrastructure.HttpClients
{
    public class RestCountriesHttpClient
    {
        private readonly HttpClient httpClient;

        public RestCountriesHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Country>> Get()
        {
            var response = await this.httpClient.GetAsync("/v3.1/all");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request GET {response.RequestMessage?.RequestUri} failed with status code {response.StatusCode} and content {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsAsync<IEnumerable<Country>>();
        }
    }
}
