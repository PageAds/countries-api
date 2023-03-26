using Countries.Domain.Models;

namespace Countries.Domain.Repositories.Interfaces
{
    public interface ICountriesRepository
    {
        Task<IEnumerable<Country>> Get();
    }
}
