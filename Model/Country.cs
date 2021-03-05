using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class Country
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
