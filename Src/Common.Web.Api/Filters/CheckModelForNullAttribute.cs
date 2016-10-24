using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common.Web.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> _validate;

        public CheckModelForNullAttribute()
            : this(arguments => arguments.ContainsValue(null))
        { }

        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _validate = checkCondition;
        }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            if (_validate(actionContext.ActionArguments))
            {
                actionContext.Response =
                    actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, ExceptionMessages.NullHttpRequestException);
                return;
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_validate(actionContext.ActionArguments))
            {
                actionContext.Response =
                    actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, ExceptionMessages.NullHttpRequestException);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
