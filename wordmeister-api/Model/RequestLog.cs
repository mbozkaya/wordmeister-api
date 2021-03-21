using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class RequestLog : BaseModel
    {
        public DateTime LogDate { get; set; }
        public int Count { get; set; }
    }
}
