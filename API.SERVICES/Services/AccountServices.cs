using API.COMMON.Configs;
using API.COMMON.Library;
using API.DATA;
using API.REPO.IRepository;
using API.SERVICES.IServices;
using API.SERVICES.Models;
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
        public AccountServices(IRepository<Account> repository)
        {
            this.repository = repository;
        }
        public AccountModel? Get(LoginRequest loginRequest)
        {
            loginRequest.Password = StringLibrary.PasswordHash(loginRequest.Password);
            var model = repository.Where(x => x.Username.Equals(loginRequest.Username) && x.Password.Equals(loginRequest.Password))
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
                UserId = model.UserId
            };

        }
        public async Task<AccountModel?> Get(int id)
        {
            var model = await repository.GetAsync(id);
            return new AccountModel
            {
                Id = model.Id,
                IsLock = model.IsLock,
                CreateAt = model.CreateAt,
                CreateBy = model.CreateBy,
                Username = model.Username,
                UserId = model.UserId,
            };
        }
        public async Task<bool> InsertAsync(AccountModel model)
        {
            var _account = Get(new LoginRequest { Username = model.Username, Password = model.Password });
            if (_account != null)
                return false;
            model.Password = StringLibrary.PasswordHash(AccountConfigs.DefaultPassword);
            var account = new Account
            {
                CreateBy = model.CreateBy,
                Password = model.Password,
                UserId = model.UserId,
                Username = model.Username,
            };
            var result = await repository.InsertAsync(account);
            if (result)
            {
                model.Id = account.Id;
                model.CreateAt = account.CreateAt;
            }
            return result;
        }
    }
}
