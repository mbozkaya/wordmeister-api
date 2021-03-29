using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using wordmeister_api.Dtos;
using wordmeister_api.Interfaces;

namespace wordmeister_api.Helpers.Classes
{
    public class Slack : INotification
    {
        private readonly Appsettings _appSettings;
        private HttpClient _httpClient;
        public Slack(IOptions<Appsettings> appSettings, HttpClient httpClient)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
        }

        public async void Message(string body, string subject = "", string to = "")
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(new { text = body }));
            await _httpClient.PostAsync(_appSettings.Slack.WebHookUrl, content);
        }
    }
}
