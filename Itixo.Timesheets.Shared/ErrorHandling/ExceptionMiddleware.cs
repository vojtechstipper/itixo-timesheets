using System;
using System.Net;
using System.Threading.Tasks;
using Itixo.Timesheets.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Itixo.Timesheets.Shared.ErrorHandling;

public static class ExceptionMiddleware
{
    public static async Task HandleException(HttpContext context)
    {
        IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (contextFeature == null)
        {
            return;
        }
        
        context.Response.ContentType = "application/json";

        if (contextFeature.Error is AppValidationException validationException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

            string failures = JsonConvert.SerializeObject(validationException.Failures);
            await context.Response.WriteAsync(
                new ErrorDetails
                {
                    StatusCode = context.Response.StatusCode,
                    Message = contextFeature.Error.Message,
                    StackTrace = contextFeature.Error.StackTrace,
                    Detail = failures
                }.ToString());

            return;
        }

        context.Response.StatusCode = (int) ResolveStatusCodeFromExceptionType(contextFeature.Error);

        await context.Response.WriteAsync(
            new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = contextFeature.Error.Message,
                StackTrace = contextFeature.Error.StackTrace
            }.ToString());
    }

    private static HttpStatusCode ResolveStatusCodeFromExceptionType(Exception exception)
    {
        if (exception is NotFoundException)
        {
            return HttpStatusCode.NotFound;
        }

        if (exception is BadRequestException)
        {
            return HttpStatusCode.BadRequest;
        }

        return HttpStatusCode.InternalServerError;
    }
}
