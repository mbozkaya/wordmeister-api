using System;
using System.Collections.Generic;
using System.Text;
using wordmeister_api.Services;
using wordmeister_api_test.Fixture;
using Xunit;

namespace wordmeister_api_test.Tests.Services
{
    public class UserServiceTest : IClassFixture<ServiceFixture>
    {
        private UserService userService;
        public UserServiceTest(ServiceFixture _serviceFixture)
        {
            userService = _serviceFixture.userService;
        }

        //public void Method_Scenario_ExpectedResult()

        [Fact]
        public void Authenticate_WithCorrectData_Success()
        {
            wordmeister_api.Dtos.Account.AuthenticateResponse response = userService.Authenticate(new wordmeister_api.Dtos.Account.AuthenticateRequest
            {
                Email = "sadfaaf@tessfsdfsdt.com",
                Password = "!&sdgdfgtest123",
            });

            string firstName = "FirstName", LastName = "LastName";

            Assert.NotNull(response);
            Assert.Equal(response.FirstName, firstName);
            Assert.Equal(response.LastName, LastName);
        }
    }
}
