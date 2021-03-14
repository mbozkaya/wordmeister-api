using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using wordmeister_api.Dtos.Account;
using wordmeister_api_test.Fixture;
using Xunit;

namespace wordmeister_api_test.Theory
{
    public class UserServiceTheory
    {
        public class AuthenticateRequestSuccess : TheoryData<AuthenticateRequest>
        {
            public AuthenticateRequestSuccess()
            {
                var config = HelperMethods.GetConfiguration();
                Add(new AuthenticateRequest
                {
                    Email = config["Mock:User:Email"],
                    Password = config["Mock:User:Password"],
                });
            }
        }

        public class AuthenticateRequestFailWithWrongEmail : TheoryData<AuthenticateRequest>
        {
            public AuthenticateRequestFailWithWrongEmail()
            {
                var config = HelperMethods.GetConfiguration();
                Add(new AuthenticateRequest
                {
                    Email = "email",
                    Password = config["Mock:User:Password"],
                });
            }
        }

        public class AuthenticateRequestFailWithWrongPassword : TheoryData<AuthenticateRequest>
        {
            public AuthenticateRequestFailWithWrongPassword()
            {
                var config = HelperMethods.GetConfiguration();
                Add(new AuthenticateRequest
                {
                    Email = config["Mock:User:Email"],
                    Password = "password",
                });
            }
        }
    }
}
