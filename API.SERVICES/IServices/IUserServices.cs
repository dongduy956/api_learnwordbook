using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.IServices
{
    public interface IUserServices
    {
        Task<UserModel?> GetAsync(int id);
        Task<bool> InsertAsync(UserModel model);

    }
}
