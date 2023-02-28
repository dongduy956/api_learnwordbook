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
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IJwtServices jwtServices;
        public AuthController(IJwtServices jwtServices)
        {
            this.jwtServices = jwtServices;
        }
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await jwtServices.GetTokenAsync(loginRequest, GetIP());
            return Ok(result);
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> Logout(JwtRequest jwtRequest)
        {
            var result = await jwtServices.RevokeRefreshTokenAsync(jwtRequest);
            return Ok(result);
        }
        [AllowAnonymous]
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
