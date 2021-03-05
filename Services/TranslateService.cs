using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Interfaces;

namespace wordmeister_api.Services
{
    public class TranslateService : ITranslateService
    {
        private TranslationClient _client;
        private IWebHostEnvironment _env;
        public TranslateService(IWebHostEnvironment env)
        {
            _env = env;
            var credentials = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile($"{_env.ContentRootPath}/My First Project-3447edb7012f.json");
            _client = TranslationClient.Create(credentials);
        }

        public string TranslateText(string text, string sourceLanguage = "en", string targetLanguage = "tr")
        {
            TranslationResult response = _client.TranslateText(
            text: text,
            targetLanguage: targetLanguage,
            sourceLanguage: sourceLanguage);

            return response.TranslatedText;
        }
    }
}
