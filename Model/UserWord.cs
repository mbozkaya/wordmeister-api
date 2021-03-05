using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class UserWord : BaseModel
    {
        [ForeignKey("UserId")]
        public long UserId { get; set; }
        [ForeignKey("WordId")]
        public long WordId { get; set; }
        public string Description { get; set; }
        public bool IsLearned { get; set; }
        public bool IsShowed { get; set; }
        public bool IsFavorite { get; set; }
        public byte Point { get; set; }
        public virtual User User { get; set; }
        public virtual Word Word { get; set; }
        public DateTime? LearnedDate { get; set; }
    }
}
