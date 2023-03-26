using Countries.Domain.Models;

namespace Countries.Domain.Repositories
{
    public interface ICountriesRepository
    {
        Task<IEnumerable<Country>> Get();
    }
}
