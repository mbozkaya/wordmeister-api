using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos
{
    public class Appsettings
    {
        public string Secret { get; set; }
        public string AESSecret { get; set; }
        public SlackSettings Slack { get; set; }
        public RapidApi RapidApi { get; set; } = new RapidApi();
        public Mail Mail { get; set; } = new Mail();
    }

    public class SlackSettings
    {
        public string WebHookUrl { get; set; }
    }


    public class RapidApi
    {
        public string XRapidapiKey { get; set; }
        public string XRapidapiHost { get; set; }
    }

    public class Mail
    {
        public string Host { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
