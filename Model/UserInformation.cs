using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class UserInformation : BaseModel
    {
        public byte FirstLanguage { get; set; }
        public byte SecondLanguage { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public long CountryId { get; set; }
        public virtual Country Country { get; set; }
        public string Phone { get; set; }
        public string SlackToken { get; set; }
    }
}
