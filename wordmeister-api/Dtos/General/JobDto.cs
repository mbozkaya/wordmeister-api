using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Model;


namespace wordmeister_api.Dtos.General
{
    public class JobDto
    {
        public class UserSentencesByWord
        {
            public long UserWordId { get; set; }
            public wordmeister_api.Model.Word Word { get; set; }
            public List<Sentence> Sentences { get; set; }
        }
    }
}
