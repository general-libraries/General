using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common.Web.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            await base.OnActionExecutingAsync(actionContext, cancellationToken);

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response =
                    actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
