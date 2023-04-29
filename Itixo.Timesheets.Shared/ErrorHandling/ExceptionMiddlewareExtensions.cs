using Microsoft.AspNetCore.Builder;

namespace Itixo.Timesheets.Shared.ErrorHandling;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                await ExceptionMiddleware.HandleException(context);
            });
        });
    }
}
