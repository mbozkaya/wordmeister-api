using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class MailUserWord
    {
        [Key]
        public long Id { get; set; }
        public long MailLogId { get; set; }
        public long UserWordId { get; set; }
        public virtual UserWord UserWord { get; set; }
        public virtual MailLog MailLog { get; set; }
    }
}
