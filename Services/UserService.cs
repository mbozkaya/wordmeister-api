using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using wordmeister_api.Dtos;
using wordmeister_api.Dtos.Account;
using wordmeister_api.Dtos.General;
using wordmeister_api.Entity;
using wordmeister_api.Helpers;
using wordmeister_api.Interfaces;
using wordmeister_api.Model;
using static wordmeister_api.Helpers.Enums;

namespace wordmeister_api.Services
{
    public class UserService : IUserService
    {
        private readonly Appsettings _appSettings;
        private WordmeisterContext _wordMeisterDbContext;
        private readonly string uploadFilePath;


        public UserService(IOptions<Appsettings> appSettings, WordmeisterContext wordMeisterDbContext)
        {
            _appSettings = appSettings.Value;
            _wordMeisterDbContext = wordMeisterDbContext;
            uploadFilePath = "UploadFiles";
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _wordMeisterDbContext.Users.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);
            var ppUri = GetUserPP(user.Id);

            return new AuthenticateResponse(user, token, ppUri);
        }

        public IEnumerable<User> GetAll()
        {
            return _wordMeisterDbContext.Users.Where(w => w.Status).ToList();
        }

        public User GetById(int id)
        {
            return _wordMeisterDbContext.Users.Where(w => w.Id == id).FirstOrDefault();
        }

        public General.ResponseResult CreateUser(SignUp model)
        {
            var user = _wordMeisterDbContext.Users.Where(w => w.Email == model.Email).FirstOrDefault();

            if (user != null)
            {
                return new General.ResponseResult() { Error = true, ErrorMessage = "There is a user that have same email" };
            }

            _wordMeisterDbContext.Users.Add(new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                CreatedDate = DateTime.Now,
                Guid = Guid.NewGuid(),
                Status = true,
            });

            _wordMeisterDbContext.SaveChanges();

            //TODO Send Email

            return new General.ResponseResult();
        }

        public General.ResponseResult UpdateInformation(AccountRequest.UpdateInformation model, long userId)
        {
            var user = _wordMeisterDbContext.Users.Where(w => w.Id == userId).FirstOrDefault();

            user.Email = model.Email;
            user.FirstName = model.Firstname;
            user.LastName = model.Lastname;
            user.UpdateDate = DateTime.Now;
            _wordMeisterDbContext.SaveChanges();

            return new General.ResponseResult();
        }

        public General.ResponseResult UpdatePassword(AccountRequest.UpdatePassword model, long userId)
        {
            var user = _wordMeisterDbContext.Users.Where(w => w.Id == userId).FirstOrDefault();

            if (user.Password != model.OldPassword)
            {
                return new General.ResponseResult
                {
                    Error = true,
                    ErrorMessage = "Old Password is wrong."
                };
            }


            if (user.Password == model.NewPassword)
            {
                return new General.ResponseResult
                {
                    Error = true,
                    ErrorMessage = "Old and new password are the same."
                };
            }


            user.Password = model.NewPassword;
            user.UpdateDate = DateTime.Now;
            _wordMeisterDbContext.SaveChanges();

            return new General.ResponseResult();
        }

        public General.ResponseResult UploadFiles(List<UploadFileDto.Request> fileModel, int userId)
        {
            var currentUser = GetById(userId);

            List<string> acceptedFileType = new List<string> { ".jpeg", ".png", ".jpg", ".bmp" };
            var isUnAcceptedFile = fileModel.Any(a => !acceptedFileType.Contains(Path.GetExtension(a.File.FileName)));
            if (isUnAcceptedFile)
            {
                return new General.ResponseResult
                {
                    Error = true,
                    ErrorMessage = "Not validating file format was found."
                };
            }

            var filesHasErrors = new List<string>();

            foreach (var item in fileModel)
            {
                if (item.File.Length < 1)
                {
                    continue;
                }

                var result = UploadFile(item, currentUser);

                if (result.error)
                {
                    filesHasErrors.Add(item.File.FileName);
                }

                if (item.Type == UploadFileType.ProfilePic)
                {
                    SetUserPP(userId, result.uploadFileId);
                }
            }

            if (filesHasErrors.Count > 0)
            {
                return new General.ResponseResult
                {
                    Error = true,
                    ErrorMessage = string.Concat(string.Join(Environment.NewLine, filesHasErrors), " The file/files have errors while uploading. "),
                };
            }

            return new General.ResponseResult();
        }

        public General.ResponseResult GetAccountInformation(int userId)
        {
            var user = GetById(userId);

            return new General.ResponseResult
            {
                Data = new AccountResponse.Information
                {
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    PictureUri = GetUserPP(userId),
                }
            };
        }

        public string GetUserPP(long userId)
        {
            var uri = _wordMeisterDbContext.UploadFiles
                .Where(w => w.UserId == userId && w.Type == (int)UploadFileType.ProfilePic && w.Status && w.IsSelected)
                .Select(s => s.Uri)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(uri))
            {
                uri = $"Files/PP/default.png";
            }

            return $"Files/{uri}";
        }

        public General.ResponseResult SetUserPP(long userId, long fileId)
        {
            var isAny = _wordMeisterDbContext.UploadFiles
                .Where(w => w.UserId == userId && w.Status && w.Id == fileId)
                .Any();

            if (!isAny)
            {
                return new General.ResponseResult { Error = true, ErrorMessage = "File was not found" };
            }

            _wordMeisterDbContext.UploadFiles
                .Where(w => w.UserId == userId && w.Status && w.Type == (int)UploadFileType.ProfilePic && w.Id != fileId)
                .ToList().ForEach(f => { f.IsSelected = false; f.UpdateDate = DateTime.Now; });

            _wordMeisterDbContext.SaveChanges();

            var selectedImage = _wordMeisterDbContext.UploadFiles.Where(w => w.Id == fileId).FirstOrDefault();
            selectedImage.IsSelected = true;
            selectedImage.UpdateDate = DateTime.Now;

            _wordMeisterDbContext.SaveChanges();

            return new General.ResponseResult();
        }

        public List<AccountResponse.UserImages> GetUserImages(int userId)
        {
            return _wordMeisterDbContext.UploadFiles
                 .Where(w => w.UserId == userId && w.Type == (int)UploadFileType.ProfilePic && w.Status)
                 .Select(s => new AccountResponse.UserImages
                 {
                     CreatedDate = s.CreatedDate,
                     Description = s.Description,
                     Id = s.Id,
                     Uri = string.Concat("Files/", s.Uri),
                     Selected = s.IsSelected,
                     Title = string.Concat(s.OriginalName, s.Extension),
                 }).ToList();
        }

        public General.ResponseResult RemoveImage(long id, long userId)
        {
            var image = _wordMeisterDbContext.UploadFiles.Where(w => w.UserId == userId && w.Id == id).FirstOrDefault();

            if (image == null)
            {
                return new General.ResponseResult
                {
                    Error = true,
                    ErrorMessage = "The image was not found",
                };
            }

            if (image.IsSelected)
            {
                return new General.ResponseResult
                {
                    Error = true,
                    ErrorMessage = "The image have selected status. Please change the selected image to remove",
                };
            }

            image.Status = false;
            image.UpdateDate = DateTime.Now;
            _wordMeisterDbContext.SaveChanges();

            return new General.ResponseResult();
        }

        private (bool error, string message, long uploadFileId) UploadFile(UploadFileDto.Request item, User user)
        {
            long uploadFileId = 0;
            var fileGuid = Guid.NewGuid();
            var fileExtension = Path.GetExtension(item.File.FileName);
            var fileUri = $"{item.Type.GetDirectoryName()}/{user.Guid.ToString("N")}";

            var filePath = $"{uploadFilePath}/{fileUri}";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            try
            {
                using (var stream = new FileStream(Path.Combine(filePath, string.Concat(fileGuid, fileExtension)), FileMode.Create))
                {
                    item.File.CopyTo(stream);
                }

                fileUri = $"{fileUri}/{fileGuid}{fileExtension}";

                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<UploadFile> newEntity = _wordMeisterDbContext.UploadFiles.Add(new Model.UploadFile
                {
                    CreatedDate = DateTime.Now,
                    Description = item.Description,
                    Extension = fileExtension,
                    Guid = fileGuid,
                    OriginalName = item.File.FileName,
                    Status = true,
                    Type = (int)item.Type,
                    Uri = fileUri,
                    UserId = user.Id,
                });
                _wordMeisterDbContext.SaveChanges();

                uploadFileId = newEntity.Entity.Id;
            }
            catch (Exception ex)
            {
                return new(true, ex.Message, uploadFileId);
            }

            return new(false, string.Empty, uploadFileId);
        }
        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
