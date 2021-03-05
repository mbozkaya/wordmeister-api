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
        IEnumerable<User> GetAll();
        User GetById(int id);
        General.ResponseResult CreateUser(SignUp model);
        General.ResponseResult UpdateInformation(AccountRequest.UpdateInformation model, long userId);
        General.ResponseResult UpdatePassword(AccountRequest.UpdatePassword model, long userId);
        General.ResponseResult UploadFiles(List<UploadFileDto.Request> fileModel, int userId);
        string GetUserPP(long userId);
        General.ResponseResult SetUserPP(long userId, long fileId);
        General.ResponseResult GetAccountInformation(int userId);
        List<AccountResponse.UserImages> GetUserImages(int userId);
        General.ResponseResult RemoveImage(long id, long userId);
    }
}
