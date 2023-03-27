using Countries.Infrastructure.Converters;
using Countries.Infrastructure.Converters.RestCountries;
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
        [JsonConverter(typeof(PropertyNamesConverter))]
        public IEnumerable<string> Currencies { get; set; }

        [JsonProperty("languages")]
        [JsonConverter(typeof(LanguagesConverter))]
        public IEnumerable<string> Languages { get; set; }

        [JsonProperty("capital")]
        public IEnumerable<string> CapitalCities { get; set; }

        [JsonProperty("borders")]
        public IEnumerable<string> Borders { get; set; }
    }
}