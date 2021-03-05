using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Helpers
{
    public static class Utilities
    {
        public static string GetAesSecretKey()
        {
            var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

            return configuration["AppSettings:AESSecret"];
        }
    }
}
