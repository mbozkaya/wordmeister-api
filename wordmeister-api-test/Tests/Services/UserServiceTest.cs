using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using wordmeister_api.Dtos.Account;
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

        // ## Defining method name pattern :===> public void Method_Scenario_ExpectedResult

        #region Method_Authenticate

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestSuccess))]
        public void Authenticate_WithCorrectData_Success(AuthenticateRequest model)
        {
            AuthenticateResponse response = userService.Authenticate(model);

            string firstName = config["Mock:User:FirstName"], LastName = config["Mock:User:LastName"];

            Assert.NotNull(response);
            Assert.Equal(response.FirstName, firstName);
            Assert.Equal(response.LastName, LastName);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestFailWithWrongEmail))]
        public void Authenticate_WithWrongEmail_Null(AuthenticateRequest model)
        {
            AuthenticateResponse response = userService.Authenticate(model);

            Assert.Null(response);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestFailWithWrongPassword))]
        public void Authenticate_WithWrongPassword_Null(AuthenticateRequest model)
        {
            AuthenticateResponse response = userService.Authenticate(model);

            Assert.Null(response);
        }


        #endregion

        #region Method_CreateUser

        [Theory]
        [ClassData(typeof(UserServiceTheory.CreateUserSuccess))]
        public void CreateUser_NewUser_Success(SignUp model)
        {
            bool expectedError = false;
            var response = userService.CreateUser(model);

            Assert.NotNull(response);
            Assert.Equal(response.Error, expectedError);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.CreateUserFailWithExistEmail))]
        public void CreateUser_ExistingUser_Fail(SignUp model)
        {
            bool expectedError = true;
            string expectedMessage = "There is a user that have same email";
            var response = userService.CreateUser(model);

            Assert.NotNull(response);
            Assert.Equal(response.Error, expectedError);
            Assert.Equal(response.ErrorMessage, expectedMessage);
        }

        #endregion

        #region Method_GetAll

        [Fact]
        public void GetAll_GetUserList_Success()
        {
            var expectedModel = new List<wordmeister_api.Model.User>();
            List<wordmeister_api.Model.User> response = userService.GetAll();

            Assert.NotNull(response);
            Assert.True(response.Count > 0);
            Assert.IsType<List<wordmeister_api.Model.User>>(response);
        }

        #endregion

        #region Method_GetById

        [Theory]
        [ClassData(typeof(UserServiceTheory.GetUserByIdExistingId))]
        public void GetById_ExistingId_Success(long id)
        {
            var response = userService.GetById(id);

            Assert.NotNull(response);
        }

        #endregion

    }
}
