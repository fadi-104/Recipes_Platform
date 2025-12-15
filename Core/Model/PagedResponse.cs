using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class PagedResponse
    {
        public int TotalCount { get; set; }
        public object Data { get; set; }
    }

    public class PagedResponse<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int FromItems { get; set; }
        public int ToItems { get; set; }
        public T Data { get; set; }
        public int PageSize { get; set; }
    }
}
