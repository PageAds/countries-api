using Newtonsoft.Json;

namespace Countries.Infrastructure.Models.RestCountriesModel
{
    public class Flags
    {
        [JsonProperty("png")]
        public string Png { get; set; }
    }
}
