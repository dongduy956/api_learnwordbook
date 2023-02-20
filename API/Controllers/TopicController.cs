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
    public class TopicController : ControllerBase
    {
        private readonly ITopicServices topicServices;
        public TopicController(ITopicServices topicServices)
        {
            this.topicServices = topicServices;
        }
        [HttpGet("[Action]")]
        public IActionResult GetAll(int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfig.PageSize;
            var result = topicServices.GetAll()
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
            var result = topicServices.Searchs(q)
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
        [HttpPost("Action")]
        public async Task<IActionResult> Insert(TopicModel model)
        {
            var result = await topicServices.InsertAsync(model);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Thêm chủ đề thành công." : "Thêm chủ đề thất bại." }
            });
        }
        [HttpPost("Action")]
        public async Task<IActionResult> InsertRange(IList<TopicModel> models)
        {
            var result = await topicServices.InsertRangeAsync(models);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Thêm chủ đề thành công." : "Thêm chủ đề thất bại." }
            });
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<IActionResult> Detele(int id)
        {
            var result = await topicServices.DeteleAsync(id);
            return Ok(new ResponseAPI
            {
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Xoá chủ đề thành công." : "Xoá chủ đề thất bại." }
            });
        }
    }
}
