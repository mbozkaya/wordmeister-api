using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class WordPronunciation : WordRelatedTableBaseModel
    {
        public string All { get; set; }
        public string Noun { get; set; }
        public string Verb { get; set; }
    }
}
