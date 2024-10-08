﻿using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace SF.Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext,e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            string message = "Unexpected error";
            if (e.GetType()==typeof(ValidationException))
            {
                message = e.Message;
            }

            if (e.GetType() == typeof(NotificationException))
            {
                message = e.Message;
            }


            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                Code = e.Source,
                Message = message
            }.ToString());
        }
    }
}
