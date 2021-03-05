using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class UserSetting : BaseModel
    {
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public byte Type { get; set; }
        public bool Enable { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
