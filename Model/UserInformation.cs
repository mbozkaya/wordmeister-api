using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class UserInformation : BaseModel
    {
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public byte FirstLanguage { get; set; }
        public byte SecondLanguage { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public string Phone { get; set; }
        public string SlackToken { get; set; }
    }
}
