using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class Country
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Iso { get; set; }
        public string NiceName { get; set; }
        public string Iso3 { get; set; }
        public int? NumCode { get; set; }
        public int? PhoneCode { get; set; }
        public virtual ICollection<UserInformation> UserInformations { get; set; }

    }
}
