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
                Messages=new string[] { !result ? "Đã có email này rồi." : "" },
                Data=model
            });
        }
        [HttpPut("[Action]/{id}")]
        public async Task<IActionResult> Update(int id,UserModel model)
        {
            var result = await userServices.UpdateAsync(id,model);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                Messages=new string[] {result?"Cập nhật tài khoản thành công.":"Có lỗi xảy ra."}
            });
        }
        [AllowAnonymous]
        [HttpDelete("[Action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userServices.DeleteAsync(id);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
            });
        }
    }
}
