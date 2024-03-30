using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace ApiCatalogo.Extensions
{
    public static class ApiExceptionsMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
       {
           appError.Run(async context =>
           {
               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
               context.Response.ContentType = "application/json";

               var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
               if (contextFeature != null)
               {
                   await context.Response.WriteAsync(new ErrorDetails()
                   {
                       StatusCode = context.Response.StatusCode,
                       Message = contextFeature.Error.Message,
                       TraceStack = contextFeature.Error.StackTrace
                   }.ToString());
               }
           });
       });


        }

    }
}