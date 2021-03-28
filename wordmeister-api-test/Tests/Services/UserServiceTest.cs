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
        private UserService _userService;
        private IConfiguration config;
        public UserServiceTest(ServiceFixture _serviceFixture)
        {
            _userService = _serviceFixture.userService;
            config = _serviceFixture.config;
        }

        // ## Defining method name pattern :===> public void Method_Scenario_ExpectedResult

        #region Method_Authenticate

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestSuccess))]
        public void Authenticate_WithCorrectData_Success(AuthenticateRequest model)
        {
            AuthenticateResponse response = _userService.Authenticate(model);

            string firstName = config["Mock:User:FirstName"], LastName = config["Mock:User:LastName"];

            Assert.NotNull(response);
            Assert.Equal(response.FirstName, firstName);
            Assert.Equal(response.LastName, LastName);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestFailWithWrongEmail))]
        public void Authenticate_WithWrongEmail_Null(AuthenticateRequest model)
        {
            AuthenticateResponse response = _userService.Authenticate(model);

            Assert.Null(response);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.AuthenticateRequestFailWithWrongPassword))]
        public void Authenticate_WithWrongPassword_Null(AuthenticateRequest model)
        {
            AuthenticateResponse response = _userService.Authenticate(model);

            Assert.Null(response);
        }


        #endregion

        #region Method_CreateUser

        [Theory]
        [ClassData(typeof(UserServiceTheory.CreateUserSuccess))]
        public void CreateUser_NewUser_Success(SignUp model)
        {
            bool expectedError = false;
            var response = _userService.CreateUser(model);

            Assert.NotNull(response);
            Assert.Equal(response.Error, expectedError);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.CreateUserFailWithExistEmail))]
        public void CreateUser_ExistingUser_Fail(SignUp model)
        {
            bool expectedError = true;
            string expectedMessage = "There is a user that have same email";
            var response = _userService.CreateUser(model);

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
            List<wordmeister_api.Model.User> response = _userService.GetAll();

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
            wordmeister_api.Model.User response = _userService.GetById(id);

            Assert.NotNull(response);
        }

        #endregion

        #region Method_UpdateInformation
        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdateInformationExistingUserId))]
        public void UpdateInformation_ExistingUser_Fail(AccountRequest.UpdateInformation model, long userId)
        {
            wordmeister_api.Dtos.General.General.ResponseResult response = _userService.UpdateInformation(model, userId);
            Assert.False(response.Error);

            var updatedUser = _userService.GetById(userId);

            Assert.Equal(model.Firstname, updatedUser.FirstName);
            Assert.Equal(model.Lastname, updatedUser.LastName);
            Assert.Equal(model.Email, updatedUser.Email);

        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdateInformationNotExistingUserId))]
        public void UpdateInformation_NotExistingUser_Fail(AccountRequest.UpdateInformation model, long userId)
        {
            string errorMessage = "User not found";
            wordmeister_api.Dtos.General.General.ResponseResult response = _userService.UpdateInformation(model, userId);

            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        #endregion

        #region Method_UpdatePassword

        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdatePasswordNewValidPassword))]
        public void UpdatePassword_ValidPassword_Success(AccountRequest.UpdatePassword model, long userId)
        {
            var response = _userService.UpdatePassword(model, userId);

            Assert.False(response.Error);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdatePasswordWrongOldPassword))]
        public void UpdatePassword_WrongOldPassword_Fail(AccountRequest.UpdatePassword model, long userId)
        {
            string errorMessage = "Old Password is wrong.";
            var response = _userService.UpdatePassword(model, userId);
            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdatePasswordSamePassword))]
        public void UpdatePassword_SameWithOldPassword_Fail(AccountRequest.UpdatePassword model, long userId)
        {
            string errorMessage = "Old and new password are the same.";
            var response = _userService.UpdatePassword(model, userId);
            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        #endregion

        #region Method_UploadFiles
        [Theory]
        [ClassData(typeof(UserServiceTheory.UploadFilesValidFileFormat))]
        public void UploadFiles_ValidFileFormat_Success(List<UploadFileDto.Request> fileModel, long userId)
        {
            var response = _userService.UploadFiles(fileModel, userId);

            Assert.False(response.Error);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.UploadFilesInvalidFileFormat))]
        public void UploadFiles_InvalidFileFormat_Fail(List<UploadFileDto.Request> fileModel, long userId)
        {
            string errorMessage = "Not validating file format was found.";
            var response = _userService.UploadFiles(fileModel, userId);

            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        #endregion

        #region Method_GetAccountInformation
        [Theory]
        [ClassData(typeof(UserServiceTheory.GetAccountInformationValidUserId))]
        public void GetAccountInformation_ValidUserId_Success(long userId)
        {
            var response = _userService.GetAccountInformation(userId);

            Assert.False(response.Error);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.GetAccountInformationInvalidUserId))]
        public void GetAccountInformation_InvalidUserId_Exception(long userId)
        {
            string errorMessage = "Object reference not set to an instance of an object.";
            Action act = () => _userService.GetAccountInformation(userId);
            var exception = Record.Exception(act);
            Assert.Equal(errorMessage, exception.Message);
        }

        #endregion

        #region Method_GetUserPP

        [Theory]
        [ClassData(typeof(UserServiceTheory.GetUserPP))]
        public void GetUserPP_ValidUserId_Default(long userId)
        {
            string uri = "Test_uri";
            var response = _userService.GetUserPP(userId);

            Assert.Equal(uri, response.Substring(response.Length - uri.Length, uri.Length));
        }

        #endregion

        #region Method_SetUserPP
        [Theory]
        [ClassData(typeof(UserServiceTheory.SetUserPPNoFile))]
        public void SetUserPP_NoFile_Fail(long userId, long fileId)
        {
            string errorMessage = "File was not found.";
            var response = _userService.SetUserPP(userId, fileId);

            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.SetUserPPWithFile))]
        public void SetUserPP_WithFile_Success(long userId, long fileId)
        {
            var response = _userService.SetUserPP(userId, fileId);

            Assert.False(response.Error);
        }

        #endregion

        #region Method_GetUserImages

        [Theory]
        [ClassData(typeof(UserServiceTheory.GetUserImagesValidUser))]
        public void GetUserImages_ValidUser_Success(long userId)
        {
            var response = _userService.GetUserImages(userId);

            Assert.NotNull(response);
            Assert.True(response.Count > 0);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.GetUserImagesInvalidUser))]
        public void GetUserImages_InvalidUser_Empty(long userId)
        {
            var response = _userService.GetUserImages(userId);

            Assert.NotNull(response);
            Assert.Empty(response);
        }

        #endregion

        #region Method_RemoveImage
        [Theory]
        [ClassData(typeof(UserServiceTheory.RemoveImageNotExistingImage))]
        public void RemoveImage_NoFile_Fail(long id, long userId)
        {
            string errorMessage = "The image was not found.";
            var response = _userService.RemoveImage(id, userId);

            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.RemoveImageUnSelectedImage))]
        public void RemoveImage_UnSelectedFile_Success(long id, long userId)
        {
            var response = _userService.RemoveImage(id, userId);

            Assert.False(response.Error);

        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.RemoveImageSelectedImage))]
        public void RemoveImage_SelectedFile_Fail(long id, long userId)
        {
            string errorMessage = "The image have selected status. Please change the selected image to remove.";
            var response = _userService.RemoveImage(id, userId);

            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        #endregion

        #region Method_UpdateSettings

        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdateSettingsEnableSetting))]
        public void UpdateSettings_SettingEnable_Success(AccountRequest.UpdateSettings model, long userId)
        {
            var response = _userService.UpdateSettings(model, userId);

            Assert.False(response.Error);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdateSettingsDisableSetting))]
        public void UpdateSettings_SettingDisable_Success(AccountRequest.UpdateSettings model, long userId)
        {
            var response = _userService.UpdateSettings(model, userId);

            Assert.False(response.Error);
        }

        [Theory]
        [ClassData(typeof(UserServiceTheory.UpdateSettingsInvalidUser))]
        public void UpdateSettings_InvalidUser_Fail(AccountRequest.UpdateSettings model, long userId)
        {
            string errorMessage = "Settings not found.";
            var response = _userService.UpdateSettings(model, userId);

            Assert.True(response.Error);
            Assert.Equal(errorMessage, response.ErrorMessage);
        }

        #endregion

        #region Method_GetSettings

        [Fact]
        public void GetSettings_ValidUser_Success()
        {
            var response = _userService.GetSettings(HelperMethods.StaticValidUserId);

            Assert.False(response.Error);
        }

        [Fact]
        public void GetSettings_InvalidUser_Exception()
        {
            string errorMessage = "Object reference not set to an instance of an object.";
            Action act = () => _userService.GetSettings(0);
            var exception = Record.Exception(act);
            Assert.Equal(errorMessage, exception.Message);
        }

        #endregion
    }
}
