using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;
using Common.LogManagement;

namespace Common.Web.Api.ExceptionHandling
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        private readonly ICommonLogger _logger;

        public ApiExceptionLogger(ICommonLogger logger)
        {
            _logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.Log(logLevel: LogLevel.Fatal, exception: context.Exception, message: RequestToString(context.Request));
        }

        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }
    }
}
