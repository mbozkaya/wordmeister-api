using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos.WordBook
{
    public class WordBookDto
    {
        public class Create
        {
            public string Title { get; set; }
            public List<string> Keys { get; set; } = new List<string>();
        }

        public class Update
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }

        public class Keyword
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string CreatedUserName { get; set; }
            public int UserId { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class KeywordAnswer
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        public class CreateAnswer : Update
        {

        }

        public class CheckAnswer : Update
        {

        }

        public class Delete
        {
            public List<int> Id { get; set; } = new List<int>();
        }
    }
}
