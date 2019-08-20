using System;

namespace VSAND.Common
{
    public static class PaginationHelp
    {
        public static string PaginationMessage(int pageNumber, int pageSize, int totalResults)
        {   
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int start = ((pageNumber - 1) * pageSize) + 1;
            int end = Math.Min(pageNumber * pageSize, totalResults);

            return string.Format("Showing {0:n0} to {1:n0} of {2:n0}", start, end, totalResults);
        }
    }
}
