using Countries.Domain.Models;

namespace Countries.Application.Models
{
    public class CountriesResponse
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<Country> Countries { get; set; }
    }
}
