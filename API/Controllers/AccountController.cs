using API.COMMON.Models;
using API.SERVICES.IServices;
using API.SERVICES.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices accountServices;
        public AccountController(IAccountServices accountServices)
        {
            this.accountServices = accountServices;
        }
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<IActionResult> Register(AccountModel model)
        {
            var result = await accountServices.InsertAsync(model);

            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                Data = result,
                Messages = new string[] { result ? "Đăng ký thành công." : "Đã có tài khoản. Đăng ký thất bại." }
            });
        }
    }
}
