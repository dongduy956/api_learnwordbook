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
    public class TopicController : ControllerBase
    {
        private readonly ITopicServices topicServices;
        public TopicController(ITopicServices topicServices)
        {
            this.topicServices = topicServices;
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAllPaging(int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfigs.PageSize;
            var result = await topicServices.GetAll()
                                      .OrderByDescending(x => x.Id)
                                      .ToPagedListAsync(page.Value, pageSize.Value);
            return Ok(new ResponseAPIPaging
            {
                StatusCode = Ok().StatusCode,
                IsSuccess = true,
                Data = result,
                PageSize = result.PageSize,
                PageCount = result.PageCount,
                PageNumber = result.PageNumber,
                TotalItems = result.TotalItemCount
            });
        }
        [HttpGet("[Action]")]
        public IActionResult GetAll()
        {
            var result = topicServices.GetAll()
                                      .OrderByDescending(x => x.Id);
            return Ok(new ResponseAPI
            {
                StatusCode = Ok().StatusCode,
                IsSuccess = true,
                Data = result,
            });
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> Search(string q, int? page, int? pageSize)
        {
            if (!page.HasValue)
                page = 1;
            if (!pageSize.HasValue)
                pageSize = PagingConfigs.PageSize;
            var result =await topicServices.Search(q)
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
        public async Task<IActionResult> Insert(TopicModel model)
        {
            var result = await topicServices.InsertAsync(model);
            return Ok(new ResponseAPI
            {
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Thêm chủ đề thành công." : "Thêm chủ đề thất bại." }
            });
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> InsertRange(IList<TopicModel> models)
        {
            var result = await topicServices.InsertRangeAsync(models);
            return Ok(new ResponseAPI
            {
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Thêm chủ đề thành công." : "Thêm chủ đề thất bại." }
            });
        }

        [HttpDelete("[Action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await topicServices.DeleteAsync(id);
            return Ok(new ResponseAPI
            {
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Xoá chủ đề thành công." : "Xoá chủ đề thất bại." }
            });
        }
        [HttpPut("[Action]/{id}")]
        public async Task<IActionResult> Update(int id,TopicModel model)
        {
            var result = await topicServices.UpdateAsync(id,model);
            return Ok(new ResponseAPI
            {
                StatusCode = result ? Ok().StatusCode : BadRequest().StatusCode,
                IsSuccess = result,
                Data = result,
                Messages = new string[] { result ? "Sửa chủ đề thành công." : "Sửa chủ đề thất bại." }
            });
        }
    }
}
