using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using wordmeister_api.Dtos.General;
using wordmeister_api.Dtos.Word;
using wordmeister_api.Helpers;
using wordmeister_api.Interfaces;
using wordmeister_api.Services;

namespace wordmeister_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //TODO: swaggerden auth kontrol edilemedigi icin simdilik yorum satırında kalacak.
    [Authorize]
    public class WordController : ControllerBase
    {
        private IWordService _wordService;
        public WordController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpPost("AddWord")]
        public IActionResult AddWord(WordRequest.Add model)
        {
            var response = _wordService.AddWord(model, User.GetUserId());
            return Ok(response);
        }

        [HttpPost("GetWord")]
        public IActionResult GetWord(IdDto model)
        {
            var response = _wordService.GetWord(model.Id, User.GetUserId());

            var result = new General.ResponseResult() { Data = response };

            return Ok(result);
        }

        [HttpPost("GetWords")]
        public IActionResult GetWords(PagingDto.Request model)
        {
            var response = _wordService.GetWords(model.PageCount, model.PageSize, User.GetUserId());

            var result = new General.ResponseResult() { Data = response };

            return Ok(result);
        }

        [HttpDelete("DeleteWord")]
        public IActionResult DeleteWord(List<long> ids)
        {
            foreach (var id in ids)
            {
                _wordService.DeleteWord(id, User.GetUserId());
            }

            return Ok(new General.ResponseResult());
        }

        [HttpPost("UpdateWord")]
        public IActionResult UpdateWord(WordRequest.Add model)
        {
            _wordService.UpdateWord(model, User.GetUserId());

            return Ok(new General.ResponseResult());
        }

        [HttpPost("WordCard")]
        public IActionResult WordCard(WordRequest.WordCard model)
        {

            return Ok(new General.ResponseResult { Data = _wordService.GetWordCard(User.GetUserId(), model.CurrentIndex, model.IsRandom) });
        }

        [HttpGet("RandomWord")]
        public IActionResult RandomWord()
        {
            _wordService.GetRandomWord();

            return Ok();
        }

        [HttpPost("WordPoint")]
        public IActionResult WordPoint(WordRequest.WordPoint model)
        {
            return Ok(_wordService.SetWordPoint(model));
        }

        [HttpPost("WordFavorite")]
        public IActionResult WordFavorite(WordRequest.WordFavorite model)
        {
            return Ok(_wordService.SetWordFavorite(model));
        }

        [HttpPost("CustomSentence")]
        public IActionResult CustomSentence(WordRequest.CustomSentence model)
        {
            return Ok(_wordService.AddCustomSentence(model));
        }

        [HttpPost("Learned")]
        public IActionResult Learned(WordRequest.Learned model)
        {
            return Ok(_wordService.SetWordLearned(model));
        }

        [HttpPost("UserWordSetting")]
        public IActionResult UserWordSetting(WordRequest.UserWordSetting model)
        {
            return Ok(_wordService.SetUserWordSetting(model, User.GetUserId()));
        }

        [HttpGet("UserWordSetting")]
        public IActionResult UserWordSetting()
        {
            return Ok(_wordService.GetUserWordSetting(User.GetUserId()));
        }
    }
}
