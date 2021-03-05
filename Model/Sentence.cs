using System.ComponentModel.DataAnnotations.Schema;

namespace wordmeister_api.Model
{
    public class Sentence : BaseModel
    {
        public string Text { get; set; }
        public long? UserId { get; set; }
        public bool IsPrivate { get; set; }
        public long WordId { get; set; }
        [ForeignKey("WordId")]
        public virtual Word Word { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
