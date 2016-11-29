using System;
using System.Collections.Generic;
using General.Configuration;
using System.Linq;

namespace General.LogManagement
{
    public class CommonLogger : ICommonLogger
    {
        private readonly IEnumerable<Lazy<ILoggingProxy>> _lazyLoggingProxies;
        private IEnumerable<ILoggingProxy> _loggingProxies;

        public IEnumerable<ILoggingProxy> LoggingProxies
        {
            get
            {
                return _loggingProxies ?? (_loggingProxies = _lazyLoggingProxies.Select(p => p.Value));
            }
        }

        /// <summary>
        /// Gets if Log Manager is automatically disabled.
        /// </summary>
        /// <value>True if automatically disabled, otherwise false.</value>
        public static bool AutomaticallyDisabled { get; private set; }

        public CommonLogger(Lazy<ILoggingProxy> lazyLoggingProxy)
            : this(new List<Lazy<ILoggingProxy>> { lazyLoggingProxy })
        { }

        public CommonLogger(IEnumerable<Lazy<ILoggingProxy>> lazyLoggingProxies)
        {
            if (lazyLoggingProxies == null) { throw new ArgumentNullException(nameof(lazyLoggingProxies)); }
            _lazyLoggingProxies = lazyLoggingProxies;
        }

        #region ICommonLogger Members

        public void Log(LogLevel logLevel, Exception exception = null, string message = "", string methodName = "", string className = "", int stackFrameIndex = 1)
        {
            try
            {
                if (AutomaticallyDisabled || SkipLoggin(logLevel))
                {
                    return;
                }

                var logEntry = new LogEntry();
                logEntry.StackTrace = new System.Diagnostics.StackTrace();
                logEntry.Level = logLevel;
                logEntry.Exception = exception;
                logEntry.MachineName = System.Environment.MachineName;
                logEntry.Message = message;
                logEntry.Date = DateTime.Now;
                //logEntry.Thread = Thread.CurrentThread;
                //logEntry.ThreadIdentity = Thread.CurrentPrincipal.Identity.Name;

                if (logEntry.StackTrace.FrameCount <= stackFrameIndex)
                {
                    stackFrameIndex = logEntry.StackTrace.FrameCount - 1;
                }

                logEntry.StackFrame = logEntry.StackTrace.GetFrame(stackFrameIndex);
                // Note: no need to check if logEntry.StackFrame != null . 
                //   We already did it in the previous if: if (logEntry.StackTrace.FrameCount <= stackFrameIndex) { ... }
                System.Reflection.MethodBase methodBase = logEntry.StackFrame.GetMethod();
                logEntry.MethodName = string.IsNullOrEmpty(methodName) ? methodBase.Name : methodName;
                logEntry.ClassName = string.IsNullOrEmpty(className) ? methodBase.DeclaringType.FullName : className;

                SaveLog(logEntry);
            }
            catch
            {
#if DEBUG
                throw;
#endif
            }
        }

        #endregion

        /// <summary>
        /// Save log to the target logging repository.
        /// </summary>
        /// <param name="logEntry"></param>
        private void SaveLog(LogEntry logEntry)
        {
            foreach (var loggingProxy in LoggingProxies)
            {
                try
                {
                    if (!loggingProxy.IsInitialized)
                    {
                        loggingProxy.Initialize(Manager.Instance.GlobalSetting.ApplicationName, Manager.Instance.GlobalSetting.VersionString);
                    }

                    loggingProxy.Save(logEntry);
                }
                catch
                {
                    //Swallow the exception;
                    //TODO: If all proxies failed, set AutomaticallyDisabled to true.
#if DEBUG
                    throw;
#endif
                }
            }
        }

        /// <summary>
        /// Returns if it should skip Logging.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns>True if should skip logging, otherwise false</returns>
        private static bool SkipLoggin(LogLevel logLevel)
        {
            return logLevel < Manager.Instance.GlobalSetting.LogLevel; //todo: or using logging section in web.config
        }
    }
}
