using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Uni.WebApi
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = 500;

            context.Response.ContentType = "application/json";
            
            var result = JsonConvert.SerializeObject(new
            {
                statuc = "error",
                message = exception.Message
            });

            await context.Response.WriteAsync(result);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    }
}