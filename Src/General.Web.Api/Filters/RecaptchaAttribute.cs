using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using General.Configuration;
using General.Web.Google.Recaptcha;

namespace General.Web.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RecaptchaAttribute : ActionFilterAttribute
    {
        public static bool Enable { get { return Manager.Instance.GlobalSetting.Recaptcha2Enable; } }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext == null && actionContext.Request == null && actionContext.Request.Headers == null)
            {
                throw new ArgumentException("actionContext is null or it does not contain HTTP request headers", nameof(actionContext));
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);

            if (await BypassRecaptach(actionContext, cancellationToken)) { return; }

            string recaptchaResponse = null;

            IEnumerable<string> recaptchaHeaderValues = actionContext.Request.Headers.FirstOrDefault(h => h.Key == "recaptcha").Value;

            if (recaptchaHeaderValues != null)
            {
                recaptchaResponse = recaptchaHeaderValues.FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(recaptchaResponse))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ExceptionMessages.ERR_RecaptchaNotFound);
            }
            else
            {
                Recaptcha2 recaptcha = new Recaptcha2();
                var recaptchaValidationResult = await recaptcha.ValidateAsync(recaptchaResponse);

                if (!recaptchaValidationResult.Success)
                {
                    string errorMessage = GenerateInvalidRecaptchaErrorMessage(recaptchaValidationResult);
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
                }
            }

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response =
                    actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }

        private async Task<bool> BypassRecaptach(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!Enable) { return await Task.FromResult(true); }

            //TODO: This is a naiv way to chekc if the code comes from an iPhone.
            //      Please a better and more secure way.
            if (actionContext != null && actionContext.Request != null && actionContext.Request.Headers != null
                //&& actionContext.Request.Headers.UserAgent != null
                // TODO: ^ uncomment the line above if you want to search the UserAgent of the request for iPhone/iPod/iPad/etc.
                )
            {
                IEnumerable<string> clientCustomVariables = actionContext.Request.Headers.FirstOrDefault(h => h.Key == "HEDigitalApiClient").Value;

                //var isIPhone =
                //    actionContext.Request.Headers.UserAgent.Any(a =>
                //        a.Comment != null &&
                //        a.Comment.Contains("Apple-iPhone"));

                if (clientCustomVariables != null && clientCustomVariables.Any(x => x == "IOS 1.0")
                    // || isIPhone
                    )
                {
                    return await Task.FromResult(true);
                }
            }

            return await Task.FromResult(false);
        }

        private string GenerateInvalidRecaptchaErrorMessage(RecaptchaValidationResult recaptchaValidationResult)
        {
            string recaptchaErrors = string.Empty;

            if (recaptchaValidationResult != null && recaptchaValidationResult.ErrorCodes != null)
            {
                recaptchaErrors = string.Join(", ", recaptchaValidationResult.ErrorCodes);

                if (!string.IsNullOrWhiteSpace(recaptchaErrors))
                {
                    recaptchaErrors = $" (Error: {recaptchaErrors})";
                }
            }

            return string.Format(ExceptionMessages.ERR_RecaptchaInvalid, recaptchaErrors);
        }
    }
}
