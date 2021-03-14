using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos.Account;
using wordmeister_api.Dtos.General;
using wordmeister_api.Model;

namespace wordmeister_api.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        List<User> GetAll();
        User GetById(long id);
        General.ResponseResult CreateUser(SignUp model);
        General.ResponseResult UpdateInformation(AccountRequest.UpdateInformation model, long userId);
        General.ResponseResult UpdatePassword(AccountRequest.UpdatePassword model, long userId);
        General.ResponseResult UploadFiles(List<UploadFileDto.Request> fileModel, long userId);
        string GetUserPP(long userId);
        General.ResponseResult SetUserPP(long userId, long fileId);
        General.ResponseResult GetAccountInformation(long userId);
        List<AccountResponse.UserImages> GetUserImages(long userId);
        General.ResponseResult RemoveImage(long id, long userId);
        General.ResponseResult UpdateSettings(AccountRequest.UpdateSettings model, long userId);
        General.ResponseResult GetSettings(long userId);
    }
}
