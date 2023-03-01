using API.COMMON.Configs;
using API.COMMON.Models;
using API.DATA;
using API.SERVICES.IServices;
using API.SERVICES.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Services
{
    public class JwtServices : IJwtServices
    {
        private readonly IAccountServices accountServices;
        private readonly ILoginSessionServices loginSessionServices;
        public JwtServices(IAccountServices accountServices, ILoginSessionServices loginSessionServices)
        {
            this.accountServices = accountServices;
            this.loginSessionServices = loginSessionServices;
        }

        public async Task<ResponseAPI> GetTokenAsync(LoginRequest loginRequest, string ipAddress)
        {
            var account = accountServices.Get(loginRequest);
            if (account == null)
                return new ResponseAPI
                {
                    IsSuccess = false,
                    Messages = new string[] { "Tài khoản hoặc mật khẩu không chính xác." }
                };
            if (account.IsLock)
                return new ResponseAPI
                {
                    IsSuccess = false,
                    Messages = new string[] { "Tài khoản bị khoá. Vui lòng liên hệ admin để biết thêm chi tiết." }
                };
            var jwtRequest = new JwtRequest
            {
                AccessToken = GenerateAccessToken(account.Id),
                RefreshToken = GenerateRefreshToken()
            };
            return await SaveToken(jwtRequest, ipAddress, account);
        }

        public bool IsTokenLive(string accessToken)
        {
            var loginSession = loginSessionServices.Get(accessToken);
            return !(loginSession == null || loginSession.IsRevoked || loginSession.IsExpired);
        }

        public async Task<ResponseAPI> RenewRefreshTokenAsync(int accountId, string ipAddress, JwtRequest jwtRequest)
        {
            var model = loginSessionServices.Get(jwtRequest);
            if (model == null)
                return new ResponseAPI
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Messages = new string[] { "Token không tìm thấy." }
                };
            if (model.IsExpired)
                return new ResponseAPI
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Messages = new string[] { "Token đã hết hạn." }
                };
            if (model.IsRevoked)
                return new ResponseAPI
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Messages = new string[] { "Token đã bị thu hồi." }
                };
            var result = await loginSessionServices.DeleteAsync(jwtRequest);
            if (!result)
                return new ResponseAPI
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Messages = new string[] { "Có lỗi xảy ra." }
                };
            var account = await accountServices.Get(accountId);
            jwtRequest.AccessToken = GenerateAccessToken(accountId);
            jwtRequest.RefreshToken = GenerateRefreshToken();
            return await SaveToken(jwtRequest, ipAddress, account);
        }
        public async Task<ResponseAPI> RevokeRefreshTokenAsync(JwtRequest jwtRequest)
        {
            if (await loginSessionServices.DeleteAsync(jwtRequest))
                return new ResponseAPI { 
                    IsSuccess=true,
                    StatusCode=(int)HttpStatusCode.OK,
                    Messages=new string[] {"Xoá thành công."}
                };
            return new ResponseAPI
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Messages = new string[] { "Xoá thất bại." }
            };
        }

        private string GenerateAccessToken(int accountId)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(JwtConfigs.SecretKey);
            var tokenSecurityHandle = new JwtSecurityTokenHandler();
            var jwtDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId,accountId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(JwtConfigs.AccessTokenTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenSecurityHandle.CreateToken(jwtDescription);
            return tokenSecurityHandle.WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
        public JwtSecurityToken? GetSecurityToken(string accsessToken)
        {
            return new JwtSecurityToken(accsessToken);
        }
        private async Task<ResponseAPI> SaveToken(JwtRequest jwtRequest, string ipAddress, AccountModel accountModel)
        {
            var tokenInfomation = GetSecurityToken(jwtRequest.AccessToken);
            var loginSession = new LoginSessionModel
            {
                AccessToken = jwtRequest.AccessToken,
                AccountId = accountModel.Id,
                CreateBy = accountModel.Username,
                Expired = DateTime.UtcNow.AddDays(JwtConfigs.RefreshTokenTime),
                IPAddress = ipAddress,
                IsRevoked = false,
                RefreshToken = jwtRequest.RefreshToken,
                TokenId = tokenInfomation.Id,
            };
            if (await loginSessionServices.InsertAsync(loginSession))
                return new ResponseAPI
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new JwtResponse
                    {
                        AccessToken = jwtRequest.AccessToken,
                        RefreshToken = jwtRequest.RefreshToken,
                        ExpiredTime = JwtConfigs.RefreshTokenTime,
                        User = accountModel
                    }
                };
            return new ResponseAPI
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
    }
}
