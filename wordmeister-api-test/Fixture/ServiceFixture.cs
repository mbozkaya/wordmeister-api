using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wordmeister_api.Dtos;
using wordmeister_api.Services;
using wordmeister_api_test.Mock.Entity;

namespace wordmeister_api_test.Fixture
{
    public class ServiceFixture : IDisposable
    {
        public MockDbContext wordMeisterDbContext { get; private set; }
        public IOptions<Appsettings> appSettings { get; set; }
        public UserService userService;
        public IConfiguration config;


        public ServiceFixture()
        {
            config = HelperMethods.GetConfiguration();
            wordMeisterDbContext = new MockDbContext();
            appSettings = Options.Create<Appsettings>(new Appsettings
            {
                AESSecret = config["AESSecret"],
                RapidApi = new RapidApi
                {
                    XRapidapiHost = "",
                    XRapidapiKey = "",
                },
                Secret = config["Secret"],
                Slack = new SlackSettings
                {
                    WebHookUrl = "",
                },
            });
            userService = new UserService(appSettings, wordMeisterDbContext);
            InsertMockDataToDB();
        }

        public void InsertMockDataToDB()
        {
            var fakeUser = new wordmeister_api.Model.User
            {
                CreatedDate = DateTime.Now,
                Email = config["Mock:User:Email"],
                Password = config["Mock:User:Password"],
                FirstName = config["Mock:User:FirstName"],
                Guid = Guid.NewGuid(),
                LastName = config["Mock:User:LastName"],
                Status = true
            };
            wordMeisterDbContext.Users.Add(fakeUser);
            wordMeisterDbContext.SaveChanges();

            var selectedFakeFile = new wordmeister_api.Model.UploadFile
            {
                CreatedDate = DateTime.Now,
                Description = "Test_desc",
                Extension = "png",
                Guid = Guid.NewGuid(),
                OriginalName = "Test_OrgName",
                Status = true,
                Type = (int)wordmeister_api.Helpers.Enums.UploadFileType.ProfilePic,
                Uri = "Test_uri",
                UserId = 1,
                IsSelected = true
            };

            var unSelectedFakeFile = new wordmeister_api.Model.UploadFile
            {
                CreatedDate = DateTime.Now,
                Description = "Test_desc_2",
                Extension = "jpg",
                Guid = Guid.NewGuid(),
                OriginalName = "Test_OrgName_2",
                Status = true,
                Type = (int)wordmeister_api.Helpers.Enums.UploadFileType.ProfilePic,
                Uri = "Test_uri",
                UserId = 1,
                IsSelected = false
            };

            wordMeisterDbContext.UploadFiles.Add(selectedFakeFile);

            wordMeisterDbContext.UploadFiles.Add(unSelectedFakeFile);
            wordMeisterDbContext.SaveChanges();

            List<wordmeister_api.Model.UserSetting> userSettings = new List<wordmeister_api.Model.UserSetting>();

            var settingTypes = Enum.GetValues(typeof(wordmeister_api.Helpers.Enums.SettingType)).Cast<wordmeister_api.Helpers.Enums.SettingType>();

            foreach (var item in settingTypes.Select((value, i) => new { value, i }))
            {
                userSettings.Add(new wordmeister_api.Model.UserSetting
                {
                    CreatedDate = DateTime.Now,
                    Enable = false,
                    UserId = 1,
                    UserSettingTypeId = item.i + 1,
                });
            }

            wordMeisterDbContext.UserSettings.AddRange(userSettings);
            wordMeisterDbContext.SaveChanges();

            wordMeisterDbContext.Countries.Add(new wordmeister_api.Model.Country
            {
                Name="Test",
                NiceName="Test",
                Iso="te",
                Iso3="te",                
            });
            wordMeisterDbContext.SaveChanges();

            wordMeisterDbContext.UserInformations.Add(new wordmeister_api.Model.UserInformation
            {
                CountryId = 1,
                NotificationHour = 0,
                NotificationMinute = 0,
                UserId = 1,
            });
            wordMeisterDbContext.SaveChanges();

        }

        #region IDisposibleInitializer

        /// <summary>
        /// Called once after running all tests in the collection #1
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ServiceFixture()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                wordMeisterDbContext.Dispose();
            }
        }

        #endregion
    }
}
