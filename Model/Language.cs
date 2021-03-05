using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class Language : BaseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
