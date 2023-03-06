using API.COMMON.Configs;
using API.COMMON.Enum;
using API.COMMON.Library;
using API.DATA;
using API.REPO.IRepository;
using API.SERVICES.IServices;
using API.SERVICES.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IRepository<Account> repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string myHostUrl = "";
        public AccountServices(IRepository<Account> repository, IHttpContextAccessor _httpContextAccessor)
        {
            this.repository = repository;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public async Task<int> ChangePassword(int id, ChangePassword model)
        {
            //-1:sai pass
            //0: xác nhận mật khẩu không chính xác
            //1: oke
            //-2:có lỗi            
            model.OldPassword = StringLibrary.PasswordHash(model.OldPassword);
            var account = repository.Where(x => x.Id == id && x.Password.Equals(model.OldPassword))
                                   .FirstOrDefault();
            if (account != null)
            {
                if (model.NewPassword.Equals(model.PrePassword))
                {
                    account.Password = StringLibrary.PasswordHash(model.NewPassword);
                    var result = await repository.UpdateAsync(account);
                    if (result)
                        return 1;
                    return -2;
                }
                return 0;
            }
            return -1;

        }

        public async Task<AccountModel?> FindAccountGoogle(string username)
        {
            var account = await repository.Where(x => x.Username.Equals(username) && x.Social == 1)
                                          .Include(x=>x.User)
                                          .FirstOrDefaultAsync();
            if (account == null)
                return null;
            return new AccountModel
            {
                Id = account.Id,
                Code = account.Code,
                CreateAt = account.CreateAt,
                CreateBy = account.CreateBy,
                IsLock = account.IsLock,
                Password = account.Password,
                Social = account.Social,
                UserId = account.UserId,
                Username = account.Username,
                Avatar=account.User.Avatar,
                FullName=account.User.FullName
            };
        }

        public async Task<bool> ForgetPassword(int id, string password)
        {
            password = StringLibrary.PasswordHash(password);
            var account = await repository.GetAsync(id);
            if (account == null)
                return false;
            account.Password = password;
            return await repository.UpdateAsync(account);
        }

        public AccountModel? Get(LoginRequest loginRequest)
        {
            myHostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            loginRequest.Password = StringLibrary.PasswordHash(loginRequest.Password);
            var model = repository.GetAll(SelectEnum.Select.NONTRASH)
                                  .Include(x => x.User)
                                  .Where(x => x.Username.Equals(loginRequest.Username) && x.Password.Equals(loginRequest.Password))
                                  .FirstOrDefault();
            if (model == null)
                return null;
            return new AccountModel
            {
                Id = model.Id,
                IsLock = model.IsLock,
                CreateAt = model.CreateAt,
                CreateBy = model.CreateBy,
                Username = model.Username,
                UserId = model.UserId,
                FullName = model.User.FullName,
                Avatar =model.Social==0 ?$"{myHostUrl}{model.User.Avatar}":model.User.Avatar,
                Social=model.Social
            };

        }
        public async Task<AccountModel?> Get(int id)
        {
            myHostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var model = await repository.Where(x=>x.Id==id)
                                        .Include(x=>x.User)
                                        .SingleOrDefaultAsync();
            if (model == null)
                return null;
            return new AccountModel
            {
                Id = model.Id,
                IsLock = model.IsLock,
                CreateAt = model.CreateAt,
                CreateBy = model.CreateBy,
                Username = model.Username,
                UserId = model.UserId,
                Code = model.Code,
                FullName = model.User.FullName,
                Avatar = model.Social == 0 ? $"{myHostUrl}{model.User.Avatar}" : model.User.Avatar,
                Social = model.Social
            };
        }

        public async Task<AccountModel?> GetByUserId(int userId)
        {
            myHostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var model = await repository.GetAll(SelectEnum.Select.NONTRASH)
                                .Select(model => new AccountModel
                                {
                                    Id = model.Id,
                                    IsLock = model.IsLock,
                                    CreateAt = model.CreateAt,
                                    CreateBy = model.CreateBy,
                                    Username = model.Username,
                                    UserId = model.UserId,
                                    FullName = model.User.FullName,
                                    Avatar =model.Social==0? $"{myHostUrl}{model.User.Avatar}":model.User.Avatar,
                                    Code = model.Code,
                                    Social=model.Social
                                })
                                .SingleOrDefaultAsync(x => x.UserId == userId);
            return model;
        }

        public async Task<bool> InsertAsync(AccountModel model)
        {
            var _account = repository.GetAll(SelectEnum.Select.NONTRASH)
                                     .SingleOrDefault(x => x.Username.Equals(model.Username.ToLower().Trim()) && x.Social == 0);
            if (_account != null)
                return false;
            model.Password = StringLibrary.PasswordHash(model.Password);
            var account = new Account
            {
                CreateBy = model.CreateBy,
                Password = model.Password,
                UserId = model.UserId,
                Username = model.Username.ToLower().Trim(),
                IsLock = false,
                Social=model.Social
            };
            var result = await repository.InsertAsync(account);
            if (result)
            {
                model.Id = account.Id;
                model.CreateAt = account.CreateAt;
            }
            return result;
        }

        public async Task<bool> UpdateCode(int id, string code)
        {
            var model = await repository.GetAsync(id);
            if (model == null)
                return false;
            model.Code = code;
            return await repository.UpdateAsync(model);
        }
    }
}
