using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Uni.Api.Core.Exceptions;
using Uni.Api.Shared.Responses;

namespace Uni.Api.Web
{
    internal sealed class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                

                var errorResponseModel = new ErrorResponseModel
                {
                    Message = ex.Message
                };

                switch (ex)
                {
                    case HttpStatusCodeException httpStatusCodeException:
                        errorResponseModel.Status = httpStatusCodeException.Status;
                        context.Response.StatusCode = httpStatusCodeException.StatusCode;
                        break;
                    default:
                        errorResponseModel.Status = "An unhandled server error has occurred.";
                        context.Response.StatusCode = 500;
                        break;
                }

                var result = JsonConvert.SerializeObject(
                    errorResponseModel
                );
                
                await context.Response.WriteAsync(result);
            }
        }
    }
}
