using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

namespace General.Web.Api
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// From: 
    ///  - http://stackoverflow.com/q/17645877/538387
    ///  - http://stackoverflow.com/a/24846821/538387
    ///  - http://stackoverflow.com/a/28146517/538387
    /// </returns>
    public class BodyAndUriParameterBinding : HttpParameterBinding
    {
        private readonly HttpConfiguration _httpConfiguration;
        private readonly IEnumerable<MediaTypeFormatter> _formatters;
        private readonly IBodyModelValidator _bodyModelValidator;

        public override bool WillReadBody { get { return true; } }

        public BodyAndUriParameterBinding(HttpParameterDescriptor descriptor)
            : base(descriptor)
        {
            _httpConfiguration = descriptor.Configuration;
            _formatters = _httpConfiguration.Formatters;
            _bodyModelValidator = _httpConfiguration.Services.GetBodyModelValidator();
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            var paramFromBody = Descriptor;
            var type = paramFromBody.ParameterType;
            var request = actionContext.ControllerContext.Request;
            var formatterLogger = new ModelStateFormatterLogger(actionContext.ModelState, paramFromBody.ParameterName);
            return ExecuteBindingAsyncCore(metadataProvider, actionContext, paramFromBody, type, request, formatterLogger, cancellationToken);
        }

        // Perf-sensitive - keeping the async method as small as possible
        private async Task ExecuteBindingAsyncCore(ModelMetadataProvider metadataProvider, HttpActionContext actionContext,
                HttpParameterDescriptor paramFromBody, Type type, HttpRequestMessage request, IFormatterLogger formatterLogger,
                CancellationToken cancellationToken)
        {
            var model = await ReadContentAsync(request, type, _formatters, formatterLogger, cancellationToken) ??
                        Activator.CreateInstance(type);

            // TODO: Intead of the rest of the code, this can be a better solution:
            /*
            var fromUriAttribute = new FromUriAttribute();
            IEnumerable<ValueProviderFactory> valueProviderFactories = fromUriAttribute.GetValueProviderFactories(_httpConfiguration);
            IModelBinder modelBinder = fromUriAttribute.GetModelBinder(_httpConfiguration, type);
            ModelBinderParameterBinding mbpb = new ModelBinderParameterBinding(Descriptor, modelBinder, valueProviderFactories);
            await mbpb.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
            var mergeToModel = actionContext.ActionArguments[Descriptor.ParameterName]; 
            // How to merge with model? */

            var routeDataValues = actionContext.ControllerContext.RouteData.Values;
            var routeParams = routeDataValues.Except(routeDataValues.Where(v => v.Key == "controller"));
            var queryStringParams = new Dictionary<string, object>(QueryStringValues(request));
            var allUriParams = routeParams.Union(queryStringParams).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var key in allUriParams.Keys)
            {
                var prop = type.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                if (prop == null)
                    continue;

                var descriptor = TypeDescriptor.GetConverter(prop.PropertyType);

                if (descriptor.CanConvertFrom(typeof(string)))
                {
                    prop.SetValue(model, descriptor.ConvertFromString(allUriParams[key] as string));
                }
            }

            // Set the merged model in the context
            SetValue(actionContext, model);

            _bodyModelValidator?.Validate(model, type, metadataProvider, actionContext, paramFromBody.ParameterName);
        }

        private Task<object> ReadContentAsync(HttpRequestMessage request, Type type,
            IEnumerable<MediaTypeFormatter> formatters, IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            var content = request.Content;

            if (content == null)
            {
                var defaultValue = MediaTypeFormatter.GetDefaultValueForType(type);
                return defaultValue == null ? Task.FromResult<object>(null) : Task.FromResult(defaultValue);
            }

            return content.ReadAsAsync(type, formatters, formatterLogger, cancellationToken);
        }

        private static IDictionary<string, object> QueryStringValues(HttpRequestMessage request)
        {
            var queryStringValues = request.RequestUri.ParseQueryString();
            return queryStringValues.Cast<string>().ToDictionary(x => x, x => (object)queryStringValues[x]);
        }
    }
}
