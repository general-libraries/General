using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.LogManagement;

namespace Common.Web.Api.Handlers
{
    public class LogHttpMessageHandler : DelegatingHandler
    {
        private ICommonLogger _commonLogger;

        public LogHttpMessageHandler(ICommonLogger commonLogger)
        {
            _commonLogger = commonLogger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_commonLogger != null && request != null)
            {
                string content = await request.Content.ReadAsStringAsync();
                string message =
                    $"HTTP^ request (v{request.Version}): [{request.Method.Method}]{request.RequestUri} content: {content}";
                var msgBuilder = new StringBuilder(message);

                if (request.Headers != null)
                {
                    foreach (var header in request.Headers)
                    {
                        if (header.Value != null)
                        {
                            msgBuilder.AppendFormat("\n{0} = {1}", header.Key, string.Join(", ", header.Value));
                        }
                    }
                }

                message = msgBuilder.ToString();
                _commonLogger.Log(LogLevel.Debug, message: message);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    HttpResponseMessage response = task.Result;

                    if (_commonLogger != null && response != null)
                    {
                        string content = "NotImplementedException"; //response.Content.ReadAsStringAsync().Result;
                        string message =
                            $"HTTP response (v{response.Version}): Status code: {response.StatusCode}, Reason phrase: {response.ReasonPhrase}, content: {content}";
                        _commonLogger.Log(LogLevel.Debug, message: message);
                    }

                    return response;
                });
        }
    }
}
