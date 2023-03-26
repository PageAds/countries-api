using Newtonsoft.Json;

namespace Countries.Infrastructure.Models.RestCountriesModel
{
    public class Country
    {
        [JsonProperty("name")]
        public Name Name { get; set; }
    }
}
