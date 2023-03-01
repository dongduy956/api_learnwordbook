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
        Task<bool> UpdateAsync(int id,UserModel model);
        Task<bool> DeleteAsync(int id);
        Task<UserModel?> FindByEmail(string email);

    }
}
