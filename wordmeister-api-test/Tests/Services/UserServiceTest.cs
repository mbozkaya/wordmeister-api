using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using wordmeister_api.Services;
using wordmeister_api_test.Fixture;
using wordmeister_api_test.Theory;
using Xunit;

namespace wordmeister_api_test.Tests.Services
{
    public class UserServiceTest : IClassFixture<ServiceFixture>
    {
        private UserService userService;
        private IConfiguration config;
        public UserServiceTest(ServiceFixture _serviceFixture)
        {
            userService = _serviceFixture.userService;
            config = _serviceFixture.config;
        }

        //public void Method_Scenario_ExpectedResult()

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestSuccess))]
        public void Authenticate_WithCorrectData_Success(wordmeister_api.Dtos.Account.AuthenticateRequest model)
        {
            wordmeister_api.Dtos.Account.AuthenticateResponse response = userService.Authenticate(model);

            string firstName = config["Mock:User:FirstName"], LastName = config["Mock:User:LastName"];

            Assert.NotNull(response);
            Assert.Equal(response.FirstName, firstName);
            Assert.Equal(response.LastName, LastName);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestFailWithWrongEmail))]
        public void Authenticate_WithWrongEmail_Null(wordmeister_api.Dtos.Account.AuthenticateRequest model)
        {
            wordmeister_api.Dtos.Account.AuthenticateResponse response = userService.Authenticate(model);

            Assert.Null(response);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestFailWithWrongPassword))]
        public void Authenticate_WithWrongPassword_Null(wordmeister_api.Dtos.Account.AuthenticateRequest model)
        {
            wordmeister_api.Dtos.Account.AuthenticateResponse response = userService.Authenticate(model);

            Assert.Null(response);
        }
    }
}
