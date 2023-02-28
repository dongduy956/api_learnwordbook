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

    }
}
