using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using wordmeister_api.Dtos.Account;
using wordmeister_api_test.Fixture;
using Xunit;
using static wordmeister_api.Helpers.Enums;

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

        public class CreateUserSuccess : TheoryData<SignUp>
        {
            public CreateUserSuccess()
            {
                Add(new SignUp
                {
                    Email = "email@test.com",
                    FirstName = "Success",
                    LastName = "Test",
                    Password = "!'^^+%^+%dfsg",
                });
            }
        }

        public class CreateUserFailWithExistEmail : TheoryData<SignUp>
        {
            public CreateUserFailWithExistEmail()
            {
                var config = HelperMethods.GetConfiguration();

                Add(new SignUp
                {
                    Email = config["Mock:User:Email"],
                    FirstName = "Fail   ",
                    LastName = "Test",
                    Password = "zxvxv",
                });
            }
        }

        public class GetUserByIdExistingId : TheoryData<long>
        {
            public GetUserByIdExistingId()
            {
                Add(HelperMethods.StaticValidUserId);
            }
        }

        public class UpdateInformationExistingUserId : TheoryData<AccountRequest.UpdateInformation, long>
        {
            public UpdateInformationExistingUserId()
            {
                var config = HelperMethods.GetConfiguration();

                Add(new AccountRequest.UpdateInformation
                {
                    Email = $"test_{config["Mock:User:Email"]}",
                    Firstname = $"test_{config["Mock:User:FirstName"]}",
                    Lastname = $"test_{config["Mock:User:LastName"]}"
                }, 1);
            }
        }

        public class UpdateInformationNotExistingUserId : TheoryData<AccountRequest.UpdateInformation, long>
        {
            public UpdateInformationNotExistingUserId()
            {
                Add(new AccountRequest.UpdateInformation(), 0);
            }
        }

        public class UpdatePasswordNewValidPassword : TheoryData<AccountRequest.UpdatePassword, long>
        {
            public UpdatePasswordNewValidPassword()
            {
                var config = HelperMethods.GetConfiguration();

                Add(new AccountRequest.UpdatePassword
                {
                    NewPassword = $"Test_{config["Mock:User:Password"]}",
                    OldPassword = config["Mock:User:Password"],
                }, 1);
            }
        }

        public class UpdatePasswordWrongOldPassword : TheoryData<AccountRequest.UpdatePassword, long>
        {
            public UpdatePasswordWrongOldPassword()
            {
                Add(new AccountRequest.UpdatePassword
                {
                    NewPassword = "",
                    OldPassword = "",
                }, 1);
            }
        }

        public class UpdatePasswordSamePassword : TheoryData<AccountRequest.UpdatePassword, long>
        {
            public UpdatePasswordSamePassword()
            {
                var config = HelperMethods.GetConfiguration();

                Add(new AccountRequest.UpdatePassword
                {
                    NewPassword = config["Mock:User:Password"],
                    OldPassword = config["Mock:User:Password"],
                }, 1);
            }
        }

        public class UploadFilesInvalidFileFormat : TheoryData<List<UploadFileDto.Request>, long>
        {
            public UploadFilesInvalidFileFormat()
            {
                var fileMock = new Mock<IFormFile>();
                var content = "Hello World from a Fake File";
                var fileName = "test.pdf";
                var ms = new MemoryStream();
                var writer = new StreamWriter(ms);
                writer.Write(content);
                writer.Flush();
                ms.Position = 0;
                fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
                fileMock.Setup(_ => _.FileName).Returns(fileName);
                fileMock.Setup(_ => _.Length).Returns(ms.Length);
                Add(new List<UploadFileDto.Request>()
                {
                    new UploadFileDto.Request
                    {
                        Description="Test",
                        File=fileMock.Object,
                        Type=UploadFileType.ProfilePic,
                    }
                }, 1);
            }
        }

        public class UploadFilesValidFileFormat : TheoryData<List<UploadFileDto.Request>, long>
        {
            public UploadFilesValidFileFormat()
            {
                var fileMock = new Mock<IFormFile>();
                var content = "";
                var fileName = "test.png";
                var ms = new MemoryStream();
                var writer = new StreamWriter(ms);
                writer.Write(content);
                writer.Flush();
                ms.Position = 0;
                fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
                fileMock.Setup(_ => _.FileName).Returns(fileName);
                fileMock.Setup(_ => _.Length).Returns(ms.Length);
                Add(new List<UploadFileDto.Request>()
                {
                    new UploadFileDto.Request
                    {
                        Description="Test",
                        File=fileMock.Object,
                        Type=UploadFileType.ProfilePic,
                    }
                }, HelperMethods.StaticValidUserId);
            }
        }

        public class GetAccountInformationValidUserId : TheoryData<long>
        {
            public GetAccountInformationValidUserId()
            {
                Add(HelperMethods.StaticValidUserId);
            }
        }

        public class GetAccountInformationInvalidUserId : TheoryData<long>
        {
            public GetAccountInformationInvalidUserId()
            {
                Add(0);
            }
        }

        public class GetUserPP : TheoryData<long>
        {
            public GetUserPP()
            {
                Add(HelperMethods.StaticValidUserId);
            }
        }

        public class SetUserPPWithFile : TheoryData<long, long>
        {
            public SetUserPPWithFile()
            {
                Add(HelperMethods.StaticValidUserId, HelperMethods.StaticSelectedFileId);
            }
        }

        public class SetUserPPNoFile : TheoryData<long, long>
        {
            public SetUserPPNoFile()
            {
                Add(HelperMethods.StaticValidUserId, 0);
            }
        }

        public class GetUserImagesValidUser : TheoryData<long>
        {
            public GetUserImagesValidUser()
            {
                Add(HelperMethods.StaticValidUserId);
            }
        }
        public class GetUserImagesInvalidUser : TheoryData<long>
        {
            public GetUserImagesInvalidUser()
            {
                Add(0);
            }
        }

        public class RemoveImageNotExistingImage : TheoryData<long, long>
        {
            public RemoveImageNotExistingImage()
            {
                Add(0, HelperMethods.StaticValidUserId);
            }
        }

        public class RemoveImageUnSelectedImage : TheoryData<long, long>
        {
            public RemoveImageUnSelectedImage()
            {
                Add(HelperMethods.StaticUnSelectedFileId, HelperMethods.StaticValidUserId);
            }
        }

        public class RemoveImageSelectedImage : TheoryData<long, long>
        {
            public RemoveImageSelectedImage()
            {
                Add(HelperMethods.StaticSelectedFileId, HelperMethods.StaticValidUserId);
            }
        }

        public class UpdateSettingsEnableSetting : TheoryData<AccountRequest.UpdateSettings, long>
        {
            public UpdateSettingsEnableSetting()
            {
                Add(new AccountRequest.UpdateSettings
                {
                    Hour = 0,
                    Minute = 5,
                    UserSettings = new List<AccountRequest.UserSettings>()
                    {
                        new AccountRequest.UserSettings
                        {
                            Enable=true,
                            Type=(int)SettingType.MailNotification
                        }
                    }
                }, HelperMethods.StaticValidUserId);
            }
        }

        public class UpdateSettingsDisableSetting : TheoryData<AccountRequest.UpdateSettings, long>
        {
            public UpdateSettingsDisableSetting()
            {
                Add(new AccountRequest.UpdateSettings
                {
                    Hour = 0,
                    Minute = 5,
                    UserSettings = new List<AccountRequest.UserSettings>()
                    {
                        new AccountRequest.UserSettings
                        {
                            Enable=false,
                            Type=(int)SettingType.MailNotification
                        }
                    }
                }, HelperMethods.StaticValidUserId);
            }
        }

        public class UpdateSettingsInvalidUser : TheoryData<AccountRequest.UpdateSettings, long>
        {
            public UpdateSettingsInvalidUser()
            {
                Add(new AccountRequest.UpdateSettings
                {
                    Hour = 0,
                    Minute = 5,
                    UserSettings = new List<AccountRequest.UserSettings>()
                    {
                        new AccountRequest.UserSettings
                        {
                            Enable=false,
                            Type=(int)SettingType.MailNotification
                        }
                    }
                }, 0);
            }
        }
    }
}
