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
        public AccountModel? Get(string username, string password)
        {
            password = StringLibrary.PasswordHash(password);
            var model = repository.Where(x => x.Username.Equals(username) && x.Password.Equals(password))
                              .FirstOrDefault();
            if (model == null)
                return null;
            return new AccountModel
            {
                Id = model.Id,
                CreateAt = model.CreateAt,
                CreateBy = model.CreateBy,
                Username = model.Username,
                UserId = model.UserId
            };

        }

        public async Task<bool> InsertAsync(AccountModel model)
        {
            model.Password = StringLibrary.PasswordHash(AccountConfig.DefaultPassword);
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
