using API.COMMON.Models;
using API.SERVICES.IServices;
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
    public class StatisticalController : ControllerBase
    {
        private readonly IStatisticalServices statisticalServices;
        public StatisticalController(IStatisticalServices statisticalServices)
        {
            this.statisticalServices = statisticalServices;
        }
        [HttpGet("[Action]")]
        public IActionResult GetTopCustomer()
        {
            var result = statisticalServices.GetTopCustomer();
            return Ok(new ResponseAPI
            {
                IsSuccess = true,
                Data = result,
                StatusCode = Ok().StatusCode
            });
        }
    }
}
