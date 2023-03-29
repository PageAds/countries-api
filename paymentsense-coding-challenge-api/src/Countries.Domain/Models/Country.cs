namespace Countries.Domain.Models
{
    public class Country
    {
        public Country(
            string name, 
            string flagUrl, 
            int population, 
            IEnumerable<string> timeZones, 
            IEnumerable<string> currencies, 
            IEnumerable<string> languages, 
            IEnumerable<string> capitalCities, 
            IEnumerable<string> borders)
        {
            this.Name = name;
            this.FlagUrl = flagUrl;
            this.Population = population;
            this.TimeZones = timeZones;
            this.Currencies = currencies;
            this.Languages = languages;
            this.CapitalCities = capitalCities;
            this.Borders = borders;
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
