using Newtonsoft.Json;

namespace Countries.Infrastructure.Models.RestCountriesModel
{
    public class Country
    {
        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("cca3")]
        public string CountryCode { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }

        [JsonProperty("timezones")]
        public IEnumerable<string> TimeZones { get; set; }

        [JsonProperty("currencies")]
        public IEnumerable<string> Currencies { get; set; }

        [JsonProperty("languages")]
        public IEnumerable<string> Languages { get; set; }

        [JsonProperty("capital")]
        public IEnumerable<string> CapitalCities { get; set; }

        [JsonProperty("borders")]
        public IEnumerable<string> Borders { get; set; }
    }
}