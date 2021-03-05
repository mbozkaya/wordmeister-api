using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos.Account
{
    public class AccountRequest
    {
        public class UpdateInformation
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
        }

        public class UpdatePassword
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
