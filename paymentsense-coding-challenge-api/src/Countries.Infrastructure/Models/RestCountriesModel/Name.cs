using Newtonsoft.Json;

namespace Countries.Infrastructure.Models.RestCountriesModel
{
    public class Name
    {
        [JsonProperty]
        public string Common { get; set; }
    }
}
