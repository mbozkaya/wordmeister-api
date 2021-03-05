using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class BaseModel
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
    }

    public class WordRelatedTableBaseModel : BaseModel
    {
        [ForeignKey("UserId")]
        public long WordId { get; set; }
        public virtual Word Word { get; set; }
    }
}
