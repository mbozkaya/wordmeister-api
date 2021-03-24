using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos.General
{
    public class General
    {
        public class ResponseResult
        {
            public bool Error { get; set; }
            public object Data { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class CacheObject
        {
            public int Count { get; set; }
            public DateTime CacheDate { get; set; }
        }
    }

}
