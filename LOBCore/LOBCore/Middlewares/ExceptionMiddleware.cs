using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOBCore.DTOs;
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
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                httpContext.Response.ContentType = "application/json";

                APIResult fooAPIResult = new APIResult()
                {
                    Status = false,
                    Message = $"發生例外異常 {ex.Message}{Environment.NewLine}{ex.StackTrace}",
                };

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(fooAPIResult));
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
