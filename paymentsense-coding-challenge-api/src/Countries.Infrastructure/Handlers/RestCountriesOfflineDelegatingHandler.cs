using System.Net;

namespace Countries.Infrastructure.Handlers
{
    public class RestCountriesOfflineDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                var offlineResponse = new HttpResponseMessage 
                {
                    Content = new StringContent(File.ReadAllText($"{Environment.CurrentDirectory}/Assets/RestCountriesResponse.json")) 
                };

                return offlineResponse;
            }

            return response;
        }
    }
}
