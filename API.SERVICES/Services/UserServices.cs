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
    public class UserServices : IUserServices
    {
        private readonly IRepository<User> repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string myHostUrl = "";
        public UserServices(IRepository<User> repository, IHttpContextAccessor _httpContextAccessor)
        {
            this.repository = repository;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public async Task<UserModel?> FindByEmail(string email)
        {
            myHostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var model = await repository.GetAll(SelectEnum.Select.NONTRASH)
                            .Include(x => x.Account)
                            .Where(x => x.Email.Equals(email.Trim().ToLower()) && x.Account != null && x.Account.Social == 0)
                            .Select(model => new UserModel
                            {
                                Avatar =model.Account.Social==0? $"{myHostUrl}{model.Avatar}":model.Avatar,
                                CreateAt = model.CreateAt,
                                CreateBy = model.CreateBy,
                                Email = model.Email,
                                FullName = model.FullName,
                                Id = model.Id,

                            })
                            .SingleOrDefaultAsync();
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await repository.GetAll(SelectEnum.Select.NONTRASH)
                                        .Include(x => x.Account)
                                        .SingleOrDefaultAsync(x => x.Id == id && x.Account == null);
            if (model == null)
                return false;
            return await repository.DeleteFromTrashAsync(model);
        }

        public async Task<UserModel?> GetAsync(int id)
        {
            myHostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var model = await repository.GetAsync(id);
            if (model == null)
                return null;
            return new UserModel
            {
                Avatar =model.Account.Social==0? $"{myHostUrl}{model.Avatar}":model.Avatar,
                CreateAt = model.CreateAt,
                CreateBy = model.CreateBy,
                Email = model.Email,
                FullName = model.FullName,
                Id = model.Id
            };
        }

        public async Task<bool> InsertAsync(UserModel model)
        {
            var _user = repository.GetAll(SelectEnum.Select.NONTRASH)
                                  .Include(x => x.Account)
                                  .SingleOrDefault(x => x.Email.Equals(model.Email.ToLower().Trim()) && x.Account != null && x.Account.Social == 0);
            if (_user != null)
                return false;
            var user = new User
            {
                CreateBy = model.CreateBy,
                Avatar = model.Avatar,
                FullName = model.FullName,
                Email = model.Email.ToLower().Trim(),
            };
            var result = await repository.InsertAsync(user);
            if (result)
            {
                model.Id = user.Id;
                model.CreateAt = user.CreateAt;
            }
            return result;
        }
        public async Task<bool> UpdateAsync(int id, UserModel model)
        {
            var user = await repository.GetAsync(id);
            user.Avatar = model.Avatar;
            user.FullName = model.FullName;
            return await repository.UpdateAsync(user);
        }
    }
}
