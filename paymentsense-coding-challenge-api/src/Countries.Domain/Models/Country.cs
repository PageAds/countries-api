namespace Countries.Domain.Models
{
    public class Country
    {
        public Country(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
