using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOBCore.BusinessObjects.Factories;
using LOBCore.DataTransferObject.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LOBCore.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                var ModelState = httpContext.Features;
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";

                APIResult apiResult = APIResultFactory.Build(false, StatusCodes.Status500InternalServerError,
                    Helpers.ErrorMessageEnum.Exception,
                    exceptionMessage:$"({ex.GetType().Name}), {ex.Message}{Environment.NewLine}{ex.StackTrace}");

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResult));
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
