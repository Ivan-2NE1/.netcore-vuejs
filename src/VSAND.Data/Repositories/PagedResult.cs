using System.Collections.Generic;

namespace VSAND.Data.Repositories
{
    public class PagedResult<T>
    {
        public List<T> Results { get; set; }
        public int TotalResults { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public PagedResult(List<T> results, int totalResults, int pageSize, int pageNumber)
        {
            Results = results;
            TotalResults = totalResults;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
