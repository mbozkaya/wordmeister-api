using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class WordDefinition : WordRelatedTableBaseModel
    {
        public string Definition { get; set; }
        public string PartOfSpeech { get; set; }
    }
}
