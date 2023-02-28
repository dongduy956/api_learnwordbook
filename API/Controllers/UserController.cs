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
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;
        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<IActionResult> Register(UserModel model)
        {
            var result = await userServices.InsertAsync(model);

            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
            });
        }
    }
}
