using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Model;

namespace wordmeister_api.Dtos.Account
{
    public class AuthenticateResponse
    {
        //public int Id { get; set; }
        public string Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Avatar { get; set; }


        public AuthenticateResponse(User user, string token, string uri = "")
        {
            Guid = user.Guid.ToString();
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Token = token;
            Avatar = uri;
        }
    }
}
