using API.COMMON.Models;
using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.IServices
{
    public interface IJwtServices
    {
        Task<ResponseAPI> GetTokenAsync(LoginRequest loginRequest,string ipAddress);
        Task<ResponseAPI> RenewRefreshTokenAsync(int accountId, string ipAddress, JwtRequest jwtRequest);
        JwtSecurityToken? GetSecurityToken(string accsessToken);
        Task<ResponseAPI> RevokeRefreshTokenAsync(JwtRequest jwtRequest);
        bool IsTokenLive(string accessToken);
    }
}
