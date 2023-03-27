namespace Countries.Domain.Models
{
    public class Country
    {
        public Country(string name, string flagUrl)
        {
            this.Name = name;
            FlagUrl = flagUrl;
        }

        public string Name { get; }

        public string FlagUrl { get; set; }
    }
}
