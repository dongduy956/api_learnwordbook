using API.COMMON.Configs;
using API.COMMON.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var index = file.FileName.LastIndexOf('.');
                var fileName = file.FileName.Substring(0, index) + DateTime.Now.Ticks + file.FileName.Substring(index);
                string directoryPath = Path.Combine(webHostEnvironment.ContentRootPath + "//wwwroot//", UploadConfigs.Image);
                string filePath = Path.Combine(directoryPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Ok(new ResponseAPI
                {
                    StatusCode = Ok().StatusCode,
                    IsSuccess = true,
                    Data = $"/{UploadConfigs.Image.Substring(0, UploadConfigs.Image.Length - 1)}/{fileName}",
                    Messages = new string[] { "Upload hình thành công." }
                });
            }
            return Ok(new ResponseAPI
            {
                StatusCode = BadRequest().StatusCode,
                IsSuccess = false,
                Messages = new string[] { "Upload hình thất bại." }
            });
        }

    }
}

