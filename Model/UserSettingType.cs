using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class UserSettingType 
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public virtual ICollection<UserSetting> UserSettings{ get; set; }

    }
}
