using System.Collections.Generic;
using System.Linq;

namespace CoreApi
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(IEnumerable<T> data, int i, int length)
        {
            Data = data.Skip((i - 1) * length).Take(length).ToList();
            Total = data.Count();
        }

        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}