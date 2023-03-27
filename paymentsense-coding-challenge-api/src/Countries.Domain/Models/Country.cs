namespace Countries.Domain.Models
{
    public class Country
    {
        public Country(string name, string flagUrl)
        {
            this.Name = name;
            this.FlagUrl = flagUrl;
        }

        public string Name { get; }

        public string FlagUrl { get; }

        public int Population { get; }

        public IEnumerable<string> TimeZones { get; }

        public IEnumerable<string> Currencies { get; }

        public IEnumerable<string> Languages { get; }

        public IEnumerable<string> CapitalCities { get; }

        public IEnumerable<string> Borders { get; }
    }
}
