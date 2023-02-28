using API.COMMON.Configs;
using API.COMMON.Library;
using API.DATA;
using API.REPO.IRepository;
using API.SERVICES.IServices;
using API.SERVICES.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task<UserModel?> GetAsync(int id)
        {
            myHostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var model = await repository.GetAsync(id);
            if (model == null)
                return null;
            return new UserModel
            {
                Avatar = $"{myHostUrl}{model.Avatar}",
                CreateAt = model.CreateAt,
                CreateBy = model.CreateBy,
                Email = model.Email,
                FullName = model.FullName,
            };
        }

        public async Task<bool> InsertAsync(UserModel model)
        {
            var user = new User
            {
                CreateBy = model.CreateBy,
                Avatar=model.Avatar,
                FullName=model.FullName,
                Email=model.Email,
            };
            var result = await repository.InsertAsync(user);
            if (result)
            {
                model.Id = user.Id;
                model.CreateAt = user.CreateAt;
            }
            return result;
        }
    }
}
