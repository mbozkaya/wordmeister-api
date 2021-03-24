using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using wordmeister_api.Dtos.Account;
using wordmeister_api.Dtos.General;
using wordmeister_api.Dtos.Word;
using wordmeister_api.Helpers;
using wordmeister_api.Interfaces;
using wordmeister_api.Services;

namespace wordmeister_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserService _userService;

        ILogger<AccountController> _logger;

        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var result = new General.ResponseResult() { Data = response };

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUp model)
        {
            return Ok(_userService.CreateUser(model));
        }

        [Authorize]
        [HttpGet("Authenticated")]
        public IActionResult Authenticated()
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("UploadFile")]
        public IActionResult UploadFile([FromForm] UploadFileDto.Request model)
        {
            return Ok(_userService.UploadFiles(new List<UploadFileDto.Request> { model }, User.GetUserId()));
        }

        [Authorize]
        [HttpPost("UserProfilePic")]
        public IActionResult UserProfilePic(IdDto model)
        {
            return Ok(_userService.SetUserPP(User.GetUserId(), model.Id));
        }

        [Authorize]
        [HttpGet("UserImages")]
        public IActionResult UserImages()
        {
            var userImages = _userService.GetUserImages(User.GetUserId());
            return Ok(new General.ResponseResult { Data = userImages });
        }

        [Authorize]
        [HttpGet("AccountInformation")]
        public IActionResult AccountInformation()
        {
            return Ok(_userService.GetAccountInformation(User.GetUserId()));
        }

        [HttpPost("RemoveFile")]
        public IActionResult RemoveFile(IdDto model)
        {
            return Ok(_userService.RemoveImage(model.Id, User.GetUserId()));
        }

        [Authorize]
        [HttpPost("UpdateInformation")]
        public IActionResult UpdateInformation(AccountRequest.UpdateInformation model)
        {
            return Ok(_userService.UpdateInformation(model, User.GetUserId()));
        }

        [Authorize]
        [HttpPost("UpdatePassword")]
        public IActionResult UpdatePassword(AccountRequest.UpdatePassword model)
        {
            return Ok(_userService.UpdatePassword(model, User.GetUserId()));
        }

        [Authorize]
        [HttpPost("Settings")]
        public IActionResult UpdateSettings(AccountRequest.UpdateSettings model) => Ok(_userService.UpdateSettings(model, User.GetUserId()));

        [Authorize]
        [HttpGet("Settings")]
        public IActionResult GetSettings() => Ok(_userService.GetSettings(User.GetUserId()));
    }
}
