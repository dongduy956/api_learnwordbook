using API.COMMON.Library;
using API.COMMON.Models;
using API.SERVICES.IServices;
using API.SERVICES.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string CODE_FORGET = "FORGET/CODE";
        private const string ID_USER_FORGET = "FORGET/ID-USER";
        private readonly IJwtServices jwtServices;
        private readonly IAccountServices accountServices;
        private readonly IUserServices userServices;
        private readonly IMailServices mailServices;
        public AuthController(IJwtServices jwtServices, IAccountServices accountServices, IUserServices userServices, IMailServices mailServices)
        {
            this.jwtServices = jwtServices;
            this.accountServices = accountServices;
            this.userServices = userServices;
            this.mailServices = mailServices;
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await jwtServices.GetTokenAsync(loginRequest, GetIP());
            return Ok(result);
        }
        [Authorize]
        [HttpPost("[Action]")]
        public async Task<IActionResult> Logout(JwtRequest jwtRequest)
        {
            var result = await jwtServices.RevokeRefreshTokenAsync(jwtRequest);
            return Ok(result);
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> RefreshToken(JwtRequest jwtRequest)
        {
            var jwtInfomation = jwtServices.GetSecurityToken(jwtRequest.AccessToken);
            var accountId = jwtInfomation.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.NameId))?.Value;
            if (string.IsNullOrEmpty(accountId))
                return BadRequest(new ResponseAPI
                {
                    IsSuccess = false,
                    StatusCode = BadRequest().StatusCode
                });
            var result = await jwtServices.RenewRefreshTokenAsync(Convert.ToInt32(accountId), GetIP(), jwtRequest);
            return Ok(result);
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> SendCode(string email)
        {
            var user = await userServices.FindByEmail(email);
            if (user != null)
            {
                var account = await accountServices.GetByUserId(user.Id);
                if (account != null)
                {
                    string code = StringLibrary.RandCode();

                    var result = await mailServices.SendMail(new Mail
                    {
                        Title = "Quên mật khẩu | Trainwordbook",
                        Body = StringLibrary.HtmlBodyMail(account.Username, code),
                        MailTo = email
                    });
                    if (result)
                    {
                        account.Code = code;
                        result = await accountServices.UpdateCode(account.Id, code);
                        if (result)
                            return Ok(new ResponseAPI
                            {
                                StatusCode = Ok().StatusCode,
                                Messages = new string[] { "Mời bạn kiểm tra email để lấy mã xác nhận." },
                                IsSuccess = true,
                                Data = account
                            });
                    }
                }
            }
            return Ok(new ResponseAPI
            {
                IsSuccess = false,
                Messages = new string[] { "E-mail không tồn tại." },
                StatusCode = BadRequest().StatusCode
            });
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> ConfirmCode(int id, string code)
        {
            var account = await accountServices.Get(id);
            var result = false;
            if (!string.IsNullOrEmpty(account.Code))
                result = account.Code.Equals(code);
            if (result)
                result = await accountServices.UpdateCode(id, "");
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                Messages = new string[] { result ? "Mã xác nhận chính xác. Mời bạn đổi mật khẩu." : "Mã xác nhận không chính xác." }
            });

        }
        [HttpPut("[Action]/{id}")]
        public async Task<IActionResult> ForgetPassword(int id, string password)
        {
            var result = await accountServices.ForgetPassword(id, password);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                Messages = new string[] { result ? "Đổi mật khẩu thành công." : "Có lỗi xảy ra." }
            });
        }
        [Authorize]
        [HttpPut("[Action]/{id}")]
        public async Task<IActionResult> ChangePassword(int id, ChangePassword model)
        {
            //-1:sai pass
            //0: xác nhận mật khẩu không chính xác
            //1: oke
            //-2:có lỗi  
            var result = await accountServices.ChangePassword(id, model);
            var message = "";
            switch (result)
            {
                case -1:
                    message = "Mật khẩu không chính xác.";
                    break;
                case 0:
                    message = "Xác nhận mật khẩu không chính xác.";
                    break;
                case 1:
                    message = "Đổi mật khẩu thành công.";
                    break;
                default:
                    message = "Có lỗi xảy ra.";
                    break;
            }
            return Ok(new ResponseAPI
            {
                IsSuccess = result == 1,
                Data = result == 1,
                Messages = new string[] { message }
            });
        }
        private string? GetIP()
        {
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers["X-Forwarded-For"]))
            {
                return HttpContext.Request.Headers["X-Forwarded-For"];
            }
            return Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}
