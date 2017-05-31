using System;
using System.Net;
using EasyEventSourcing.Aggregate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace VotingApp.Api
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app) =>
            app.UseExceptionHandler(
                builder => builder.Run(
                  async context =>
                    {
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";

                            var result = "";
                            if (error.Error is DomainException || error.Error is ArgumentNullException)
                            {
                                result = JsonConvert.SerializeObject(new { error = error.Error.Message });
                            }

                            await context.Response.WriteAsync(result).ConfigureAwait(false);
                        }
                    })
                );
    }
}