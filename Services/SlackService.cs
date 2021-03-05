using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using wordmeister_api.Dtos;
using wordmeister_api.Interfaces;

namespace wordmeister_api.Services
{
    public class SlackService : ISlackService
    {
        private readonly Appsettings _appSettings;
        private HttpClient _httpClient;
        public SlackService(IOptions<Appsettings> appSettings, HttpClient httpClient)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
        }

        public async void PostMessage(object message)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(new { text = message }));
            await _httpClient.PostAsync(_appSettings.Slack.WebHookUrl, content);
        }
    }
}
