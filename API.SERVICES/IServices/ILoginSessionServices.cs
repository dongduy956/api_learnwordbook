using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.IServices
{
   public interface ILoginSessionServices
    {
        LoginSessionModel? Get(JwtRequest jwtRequest);
        LoginSessionModel? Get(string accessToken);
        Task<bool> InsertAsync(LoginSessionModel model);
        Task<bool> DeleteAsync(JwtRequest jwtRequest);
    }
}
