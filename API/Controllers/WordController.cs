using API.COMMON.Configs;
using API.COMMON.Models;
using API.SERVICES.IServices;
using API.SERVICES.Models;
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
    public class WordController : ControllerBase
    {
        private readonly IWordServices wordServices;
        public WordController(IWordServices wordServices)
        {
            this.wordServices = wordServices;
        }
        [HttpGet("[Action]")]
        public IActionResult GetAll()
        {
            var result = wordServices.GetAll()
                                      .OrderByDescending(x => x.Id);                                      
            return Ok(new ResponseAPIPaging
            {
                StatusCode = Ok().StatusCode,
                IsSuccess = true,
                Data = result,                
            });
        }
        [HttpGet("[Action]")]
        public IActionResult GetAllPaging(int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfig.PageSize;
            var result = wordServices.GetAll()
                                      .OrderByDescending(x => x.Id)
                                      .ToPagedList(page.Value, pageSize.Value);
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
        [HttpGet("[Action]")]
        public IActionResult GetWordsByTopicId(int topicId,int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfig.PageSize;
            var result = wordServices.GetWordsByTopicId(topicId)
                                      .OrderByDescending(x => x.Id)
                                      .ToPagedList(page.Value, pageSize.Value);
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
        [HttpGet("[Action]")]
        public IActionResult Searchs(string q, int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfig.PageSize;
            var result = wordServices.Searchs(q)
                                      .OrderByDescending(x => x.Id)
                                      .ToPagedList(page.Value, pageSize.Value);
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
        public async Task<IActionResult> Insert(WordModel model)
        {
            var result = await wordServices.InsertAsync(model);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Thêm từ vựng thành công." : "Thêm từ vựng thất bại." }              
            });
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> InsertRange(IList<WordModel> models)
        {
            var result = await wordServices.InsertRangeAsync(models);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Thêm từ vựng thành công." : "Thêm từ vựng thất bại." }
            });
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<IActionResult> Detele(int id)
        {
            var result = await wordServices.DeteleAsync(id);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Xoá từ vựng thành công." : "Xoá từ vựng thất bại." }
            });
        }
    }
}
