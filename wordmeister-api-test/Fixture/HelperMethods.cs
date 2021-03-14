using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace wordmeister_api_test.Fixture
{
    public static class HelperMethods
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("xunit.runner.json")
                .Build();
            return config;
        }
    }
}
