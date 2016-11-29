using System;
using System.Web;

namespace General.Web.Helpers
{
    public static class IpHelper
    {
        public static string GetClientIp(HttpRequest httpRequest = null)
        {
            if(httpRequest == null)
            {
                if(HttpContext.Current!=null && HttpContext.Current.Request!=null)
                {
                    httpRequest = HttpContext.Current.Request;
                }
                else
                {
                    throw new ArgumentException("httpRequest");
                }
            }

            return httpRequest.UserHostAddress;
        }
    }
}
