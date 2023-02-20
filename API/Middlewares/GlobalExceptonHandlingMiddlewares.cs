using API.COMMON.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public class GlobalExceptonHandlingMiddlewares
    {
        private readonly ILogger<GlobalExceptonHandlingMiddlewares> logger;
        private readonly RequestDelegate next;
        public GlobalExceptonHandlingMiddlewares(ILogger<GlobalExceptonHandlingMiddlewares> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                var response = new ResponseAPI
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Messages = new string[] { ex.Message }
                }.ToString();

                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(response);
            }
        }

    }
}

