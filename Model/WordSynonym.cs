using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class WordSynonym : WordRelatedTableBaseModel
    {
        public string Synonym { get; set; }
    }
}
