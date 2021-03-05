using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos.Account
{
    public class AccountResponse
    {
        public class Information
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
            public string PictureUri { get; set; }
        }

        public class UserImages
        {
            public string Uri { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public long Id { get; set; }
            public bool Selected { get; set; }
            public string Title { get; set; }
        }
    }
}
