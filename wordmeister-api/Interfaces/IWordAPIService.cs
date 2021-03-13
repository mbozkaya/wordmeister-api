using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos.Word;

namespace wordmeister_api.Interfaces
{
    public interface IWordAPIService
    {
        Task<WordApiResponse.RandomDto> GetWord(string word);
        Task<WordApiResponse.ExampleDto> GetExample(string word);
        Task<WordApiResponse.SynonymsDto> GetSynonyms(string word);
        Task<WordApiResponse.DefinationsDto> GetDefinations(string word);
        Task<WordApiResponse.AntonymsDto> GetAntonyms(string word);
        Task<WordApiResponse.RyhmesDto> GetRyhmes(string word);
        Task<WordApiResponse.PronunciationDto> GetPronunciation(string word);
        Task<WordApiResponse.SyllablesDto> GetSyllables(string word);
        Task<WordApiResponse.FrequencyDto> GetFrequency(string word);
        Task<WordApiResponse.RandomDto> GetRandom();
    }
}
