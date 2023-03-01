using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.IServices
{
    public interface IAccountServices
    {
        Task<AccountModel?> Get(int id);
        AccountModel? Get(LoginRequest loginRequest);
        Task<bool> InsertAsync(AccountModel model);
        Task<int> ChangePassword(int id,ChangePassword model);
        Task<AccountModel?> GetByUserId(int userId);
        Task<bool> ForgetPassword(int id,string password);
        Task<bool> UpdateCode(int id,string code);
    }
}
