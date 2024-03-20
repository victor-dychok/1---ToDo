using Common.BL;
using Common.BL.Exeptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Api
{
    public class ExeptionHendlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExeptionHendlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception e)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var result = string.Empty;
                switch (e)
                {
                    case NotFoundExeption badRequestException:
                        statusCode = HttpStatusCode.NotFound;
                        result = JsonSerializer.Serialize(badRequestException.Message);
                        break;
                    case BadRequestExeption badRequestException:
                        statusCode = HttpStatusCode.BadRequest;
                        result = JsonSerializer.Serialize(badRequestException.Message);
                        break;
                    case ForbidenExeption forbidenExeption:
                        statusCode = HttpStatusCode.Forbidden;
                        result = JsonSerializer.Serialize(forbidenExeption.Message);
                        break;
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    result = JsonSerializer.Serialize(new { error = e.Message, innerMessage = e.InnerException?.Message, e.StackTrace });
                }

                httpContext.Response.StatusCode = (int)statusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}
