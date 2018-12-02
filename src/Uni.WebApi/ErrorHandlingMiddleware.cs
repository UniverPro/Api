using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Uni.Core.Exceptions;

namespace Uni.WebApi
{
    internal sealed class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                context.Response.StatusCode = (int) ex.StatusCode;
                context.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(
                    new
                    {
                        status = "error",
                        message = ex.Message
                    }
                );

                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(
                    new
                    {
                        status = "error",
                        message = ex.Message
                    }
                );

                await context.Response.WriteAsync(result);
            }
        }
    }
}
