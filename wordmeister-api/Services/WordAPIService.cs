using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using wordmeister_api.Dtos;
using wordmeister_api.Dtos.General;
using wordmeister_api.Dtos.Word;
using wordmeister_api.Entity;
using wordmeister_api.Interfaces;

namespace wordmeister_api.Services
{
    public class WordAPIService : IWordAPIService
    {
        HttpClient _client;
        private HttpRequestMessage _request;
        IOptions<Appsettings> _appSettings;
        private HttpRequestException _httpRequestException;
        private bool _error;
        private string _baseUri = "https://wordsapiv1.p.rapidapi.com/words/";
        WordmeisterContext _dbContext;
        public WordAPIService(IOptions<Appsettings> appSettings, WordmeisterContext dbContext)
        {
            _appSettings = appSettings;
            _client = new HttpClient();
            _dbContext = dbContext;
            _request = new HttpRequestMessage
            {
                Headers =
                        {
                            { "x-rapidapi-key", _appSettings.Value.RapidApi.XRapidapiKey },
                            { "x-rapidapi-host", _appSettings.Value.RapidApi.XRapidapiHost },
                            { "useQueryString", "true" },
                        },
            };
            _request.Method = HttpMethod.Get;

        }
        public async Task<WordApiResponse.RandomDto> GetWord(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}");

            var result = await SendRequest<WordApiResponse.RandomDto>();

            return result;
        }

        public async Task<WordApiResponse.ExampleDto> GetExample(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}/examples");

            var response = await SendRequest<WordApiResponse.ExampleDto>();

            return response;
        }
        /// <summary>
        /// Eş anlamlıları döner
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<WordApiResponse.SynonymsDto> GetSynonyms(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}/synonyms");

            var response = await SendRequest<WordApiResponse.SynonymsDto>();

            return response;
        }
        /// <summary>
        /// Tanımlamaları döner
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<WordApiResponse.DefinationsDto> GetDefinations(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}/definitions");

            var response = await SendRequest<WordApiResponse.DefinationsDto>();

            return response;
        }
        /// <summary>
        /// Zıt anlamlılarını döner
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<WordApiResponse.AntonymsDto> GetAntonyms(string word)
        {
            _request.RequestUri = new Uri($"https://wordsapiv1.p.rapidapi.com/words/{word}/antonyms");

            var response = await SendRequest<WordApiResponse.AntonymsDto>();

            return response;
        }
        /// <summary>
        /// Tekerlemeleri döner
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<WordApiResponse.RyhmesDto> GetRyhmes(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}/rhymes");

            var response = await SendRequest<WordApiResponse.RyhmesDto>();

            return response;
        }
        /// <summary>
        /// Okunuşları döner
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<WordApiResponse.PronunciationDto> GetPronunciation(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}/pronunciation");

            var response = await SendRequest<WordApiResponse.PronunciationDto>();

            return response;
        }
        /// <summary>
        /// Heceleri döner
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<WordApiResponse.SyllablesDto> GetSyllables(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}/syllables");

            var response = await SendRequest<WordApiResponse.SyllablesDto>();

            return response;
        }

        public async Task<WordApiResponse.FrequencyDto> GetFrequency(string word)
        {
            _request.RequestUri = new Uri($"{_baseUri}{word}/frequency");

            var response = await SendRequest<WordApiResponse.FrequencyDto>();

            return response;
        }

        public async Task<WordApiResponse.RandomDto> GetRandom()
        {
            _request.RequestUri = new Uri($"{_baseUri}?random=true");
            var response = await SendRequest<WordApiResponse.RandomDto>();

            return response;
        }

        private async Task<T> SendRequest<T>() where T : new()
        {
            RequestLimit();
            var responseResult = new T();
            using (var response = await _client.SendAsync(_request))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        var responseResultStr = await response.Content.ReadAsStringAsync();
                        var jsonObject = JObject.Parse(responseResultStr);

                        if (typeof(T).Name == "RandomDto" && (jsonObject["pronunciation"] == null || jsonObject["pronunciation"].Type == JTokenType.String))
                        {
                            jsonObject["pronunciation"] = JsonConvert.SerializeObject(new WordApiResponse.Pronunciation() { All = jsonObject["pronunciation"] != null ? jsonObject["pronunciation"].ToString() : string.Empty, Verb = "", Noun = "" });
                            responseResultStr = jsonObject.ToString().Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}");
                        }

                        if (typeof(T).Name == "RandomDto" && jsonObject["syllables"] == null)
                        {
                            jsonObject["syllables"] = JsonConvert.SerializeObject(new WordApiResponse.Syllables { Count = 0, List = new List<string>() });
                            responseResultStr = jsonObject.ToString().Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}");
                        }

                        if (typeof(T).Name == "RandomDto" && jsonObject["frequency"] == null)
                        {
                            jsonObject["frequency"] = JsonConvert.SerializeObject(decimal.Zero);
                            responseResultStr = jsonObject.ToString().Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}");
                        }
                        responseResult = JsonConvert.DeserializeObject<T>(responseResultStr);

                    }
                    catch (HttpRequestException httpEx)
                    {
                        //TODO Log
                        _httpRequestException = httpEx;
                        _error = true;
                    }
                    catch (Exception ex)
                    {
                        //TODO Log
                        _error = true;
                    }
                }
                else
                {
                    //TODO Log
                    _error = true;
                }
            }
            return responseResult;
        }

        private void RequestLimit()
        {
            var counter = _dbContext.RequestLogs
                .Where(w => w.LogDate.Date == DateTime.Now.Date)
                .FirstOrDefault();

            if (counter == null)
            {
                _dbContext.RequestLogs.Add(new Model.RequestLog
                {
                    Count = 1,
                    LogDate = DateTime.Now.Date,
                    CreatedDate = DateTime.Now,
                });
                _dbContext.SaveChanges();
            }
            else if (counter.Count > 2400)
            {
                throw new Exception("Beta limit exceeded.");
            }
        }
    }
}
