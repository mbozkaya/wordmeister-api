using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class WordDerivation : WordRelatedTableBaseModel
    {
        public string Derivation { get; set; }
    }
}
