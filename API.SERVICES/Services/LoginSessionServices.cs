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
    public class LoginSessionServices : ILoginSessionServices
    {
        private readonly IRepository<LoginSession> repository;
        public LoginSessionServices(IRepository<LoginSession> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await repository.GetAsync(id);
            if (model == null)
                return false;
            return await repository.DeleteFromTrashAsync(model);
        }

        public LoginSessionModel? Get(string accessToken, string refreshToken)
        {
            var model = repository.Where(x => x.AccessToken.Equals(accessToken) && x.RefreshToken.Equals(refreshToken) && x.IsExpired)
                                    .FirstOrDefault();
            if (model == null)
                return null;
            return new LoginSessionModel
            {
                AccessToken = model.AccessToken,
                AccountId = model.AccountId,
                CreateAt = model.CreateAt,
                CreateBy = model.CreateBy,
                Expired = model.Expired,
                IPAddress = model.IPAddress,
                Id = model.Id,
                IsRevoked = model.IsRevoked,
                RefreshToken = model.RefreshToken,
                TokenId = model.TokenId
            };
        }

        public async Task<bool> InsertAsync(LoginSessionModel model)
        {
            var loginSession = new LoginSession
            {
                TokenId = model.TokenId,
                RefreshToken = model.RefreshToken,
                IsRevoked = false,
                IPAddress = model.IPAddress,
                Expired = model.Expired,
                CreateBy = model.CreateBy,
                AccountId = model.AccountId,
                AccessToken = model.AccessToken
            };
            var result = await repository.InsertAsync(loginSession);
            if (result)
            {
                model.CreateAt = loginSession.CreateAt;
                model.Id = loginSession.Id;
            }
            return result;
        }
    }
}
