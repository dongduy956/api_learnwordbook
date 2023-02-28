using API.COMMON.Configs;
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
using X.PagedList;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LearnedWordController : ControllerBase
    {
        private readonly ILearnedWordServices learnedWordServices;
        public LearnedWordController(ILearnedWordServices learnedWordServices)
        {
            this.learnedWordServices = learnedWordServices;
        }
        [HttpGet("[Action]/{accountId}")]
        public IActionResult GetAll(int accountId)
        {
            var result = learnedWordServices.GetAll(accountId)
                                      .OrderByDescending(x => x.Id);
            return Ok(new ResponseAPI
            {
                StatusCode = Ok().StatusCode,
                IsSuccess = true,
                Data = result,
            });
        }
        [HttpGet("[Action]/{accountId}")]
        public IActionResult GetAllIncorrect(int accountId)
        {
            var result = learnedWordServices.GetAllIncorrect(accountId)
                                      .OrderByDescending(x => x.Id);
            return Ok(new ResponseAPI
            {
                StatusCode = Ok().StatusCode,
                IsSuccess = true,
                Data = result,
            });
        }
        [HttpGet("[Action]/{accountId}")]
        public async Task<IActionResult> GetAllPaging(int accountId,int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfigs.PageSize;
            var result =await learnedWordServices.GetAll(accountId)
                                      .OrderByDescending(x => x.Id)
                                      .ToPagedListAsync(page.Value, pageSize.Value);
            return Ok(new ResponseAPIPaging
            {
                StatusCode = Ok().StatusCode,
                IsSuccess = true,
                Data = result,
                PageCount = result.PageCount,
                PageSize = result.PageSize,
                PageNumber = result.PageNumber,
                TotalItems = result.TotalItemCount
            });
        }
        [HttpGet("[Action]/{accountId}")]
        public async Task<IActionResult> Search(int accountId,string q, int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfigs.PageSize;
            var result =await learnedWordServices.Search(accountId,q)
                                      .OrderByDescending(x => x.Id)
                                      .ToPagedListAsync(page.Value, pageSize.Value);
            return Ok(new ResponseAPIPaging
            {
                StatusCode = Ok().StatusCode,
                IsSuccess = true,
                Data = result,
                PageCount = result.PageCount,
                PageSize = pageSize.Value,
                PageNumber = result.PageNumber,
                TotalItems = result.TotalItemCount
            });
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> InsertRange(IList<LearnedWordModel> models)
        {
            var result = await learnedWordServices.InsertRangeAsync(models);
            return Ok(new ResponseAPI
            {
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Thêm từ đã học thành công." : "Thêm từ đã học thất bại." }
            });
        }
        [HttpPut("[Action]")]
        public async Task<IActionResult> UpdateRange(IList<LearnedWordModel> models)
        {
            var result = await learnedWordServices.UpdateRangeAsync(models);
            return Ok(new ResponseAPI
            {
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Sửa từ đã học thành công." : "Sửa từ đã học thất bại." }
            });
        }
    }
}
