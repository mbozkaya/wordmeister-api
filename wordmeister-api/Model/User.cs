using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace wordmeister_api.Model
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Comment("EncryptedField")]
        public string Email { get; set; }
        [Comment("EncryptedField")]
        public string Password { get; set; }
        public Guid Guid { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<UserWord> UserWords { get; set; }
        public virtual ICollection<Sentence> Sentences { get; set; }
        public virtual ICollection<UserInformation> UserInformations { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
        public virtual ICollection<UserWordSetting> UserWordSettings { get; set; }
    }
}
