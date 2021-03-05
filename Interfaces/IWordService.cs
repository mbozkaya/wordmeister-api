using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos.Word;
using static wordmeister_api.Dtos.General.General;

namespace wordmeister_api.Interfaces
{
    public interface IWordService
    {
        WordResponse.Word GetWord(long wordId, int userId);
        PageResponse GetWords(int skipRows, int pageSize, int userId);
        ResponseResult AddWord(WordRequest.Add model, int userId);
        void UpdateWord(WordRequest.Add model, int userId);
        void DeleteWord(long wordId, int userId);
        WordResponse.WordCard GetWordCard(int userId, int currentCount = 0, bool isRandom = false);
        void GetRandomWord();
        ResponseResult SetWordPoint(WordRequest.WordPoint model);
        ResponseResult SetWordFavorite(WordRequest.WordFavorite model);
        ResponseResult AddCustomSentence(WordRequest.CustomSentence model);
        ResponseResult SetWordLearned(WordRequest.Learned model);
        ResponseResult SetUserWordSetting(WordRequest.UserWordSetting model, int userId);
        ResponseResult GetUserWordSetting(int userId);
    }
}
