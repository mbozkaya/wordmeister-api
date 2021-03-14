using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using wordmeister_api.Dtos;
using wordmeister_api.Services;
using wordmeister_api_test.Mock.Entity;

namespace wordmeister_api_test.Fixture
{
    public class ServiceFixture : IDisposable
    {
        public MockDbContext wordmeisterDbContext { get; private set; }
        public IOptions<Appsettings> appSettings { get; set; }
        public UserService userService;
        public IConfiguration config;


        public ServiceFixture()
        {
            config = HelperMethods.GetConfiguration();
            wordmeisterDbContext = new MockDbContext();
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
            userService = new UserService(appSettings, wordmeisterDbContext);
            InsertMockDataToDB();
        }

        public void InsertMockDataToDB()
        {
            wordmeisterDbContext.Users.Add(new wordmeister_api.Model.User
            {
                CreatedDate = DateTime.Now,
                Email = config["Mock:User:Email"],
                Password = config["Mock:User:Password"],
                FirstName = config["Mock:User:FirstName"],
                Guid = Guid.NewGuid(),
                LastName = config["Mock:User:LastName"],
                Status = true
            });
            wordmeisterDbContext.SaveChanges();
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
                wordmeisterDbContext.Dispose();
            }
        }

        #endregion
    }
}
