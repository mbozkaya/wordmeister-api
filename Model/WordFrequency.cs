using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class WordFrequency : WordRelatedTableBaseModel
    {
        public decimal Zipf { get; set; }
        public decimal PerMillion { get; set; }
        public decimal Diversity { get; set; }
    }
}
