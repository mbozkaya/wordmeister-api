using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class MailLog
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public virtual User Users { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<MailUserWord> MailUserWords { get; set; }

    }
}
