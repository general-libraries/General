using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace General.Web.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class FromBodyAndUriAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new BodyAndUriParameterBinding(parameter);
        }
    }
}
