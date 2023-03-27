namespace Countries.Api.Filters
{
    public class PaginationFilter
    {
        private const int MaxPageSize = 20;

        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = MaxPageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
