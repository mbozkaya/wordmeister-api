using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos.General
{
    public class PagingDto
    {
        public class Request
        {
            public Request()
            {
                PageCount = 1;
                PageSize = 50;
                Order = string.Empty;
                OrderBy = string.Empty;
            }
            public int PageCount { get; set; }
            public int PageSize { get; set; }
            public string OrderBy { get; set; }
            public string Order { get; set; }
        }

        public class Response<T>
        {
            public int TotalCount { get; set; }
            public int PageCount { get; set; }
            public List<T> Data { get; set; }
        }
    }
}
