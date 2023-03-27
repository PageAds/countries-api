using Newtonsoft.Json;

namespace Countries.Infrastructure.Models.RestCountriesModel
{
    public class Country
    {
        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }
    }
}
