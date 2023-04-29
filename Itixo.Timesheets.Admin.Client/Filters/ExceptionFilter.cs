using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using System;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    protected override Task OnPageExceptionAsync(IDotvvmRequestContext context, Exception exception)
    {
        context.IsPageExceptionHandled = true;
        context.RedirectToUrl("/500");
        return base.OnPageExceptionAsync(context, exception);
    }
}
