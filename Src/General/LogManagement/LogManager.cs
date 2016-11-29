using System;
using System.Collections.Generic;
using System.Linq;
using General.Configuration;

namespace General.LogManagement
{
    //TODO: maybe rewrite it with singleton patter?!

    /// <summary>
    /// Encapsulates functionalities for logging.
    /// </summary>
    public static class LogManager
    {
        #region private fields
        /// <summary>
        /// The Application Name to be used for logging messages
        /// </summary>
        private static string _appName;
        /// <summary>
        /// Is LogManager ever Initialized?
        /// </summary>
        private static bool _isInitialized = false;

        private static IList<ILoggingProxy> _loggingProxies;

        private static Object _initLock = new Object();
        #endregion

        /// <summary>
        /// Gets if Log Manager is automatically disabled.
        /// </summary>
        /// <value>True if automatically disabled, otherwise false.</value>
        public static bool AutomaticallyDisabled { get; private set; }

        /// <summary>
        /// Gets if LogManager has already initialized.
        /// </summary>
        public static bool IsInitialized { get { return _isInitialized; } }

        /// <summary>
        /// Logs an event to the log repository.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        /// <param name="className"></param>
        /// <param name="stackFrameIndex"></param>
        public static void Log(LogLevel logLevel, Exception exception = null, string message = "", string methodName = "", string className = "", int stackFrameIndex = 1)
        {
            try
            {
                if (!_isInitialized)
                {
                    Initialize();
                }

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
                //Note no need to check if logEntry.StackFrame != null . We already did it in the previous if: if (logEntry.StackTrace.FrameCount <= stackFrameIndex) { ... }
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

        /// <summary>
        /// Initializes the LogManager.
        /// </summary>
        /// <param name="loggingProxy">a logProxy object.</param>
        /// <returns>True if succesfully initialized, otherwise false.</returns>
        public static bool Initialize(ILoggingProxy loggingProxy)
        {
            if (loggingProxy == null) { throw new ArgumentNullException(nameof(loggingProxy)); }
            return Initialize(null, null, new List<ILoggingProxy> { loggingProxy });
        }

        /// <summary>
        /// Initializes the LogManager.
        /// </summary>
        /// <param name="applicationName">Name of current application.</param>
        /// <param name="applicationVersion">Version of current application.</param>
        /// <param name="loggingProxies">a list of logProxy objects.</param>
        /// <returns>True if succesfully initialized, otherwise false.</returns>
        public static bool Initialize(string applicationName = null, string applicationVersion = null, IEnumerable<ILoggingProxy> loggingProxies = null)
        {
            if (loggingProxies == null || !loggingProxies.Any()) { return false; }
            _isInitialized = false;

            lock (_initLock)
            {
                try
                {
                    _appName = applicationName ?? Manager.Instance.GlobalSetting.ApplicationName;
                    _loggingProxies = loggingProxies.ToList();

                    foreach (var loggingProxy in _loggingProxies)
                    {
                        try
                        {
                            loggingProxy.Initialize(_appName, applicationVersion ?? Manager.Instance.GlobalSetting.VersionString);
                            _isInitialized = true;
                        }
                        catch
                        {
                            //Swallow the exception in the Release mode.
#if DEBUG
                            throw;
#endif
                        }
                    }

                    AutomaticallyDisabled = !_isInitialized;
                }
                catch
                {
#if DEBUG
                    throw;
#endif
                }
            }

            return _isInitialized;
        }

        /// <summary>
        /// Save log to the target logging repository.
        /// </summary>
        /// <param name="logEntry"></param>
        private static void SaveLog(LogEntry logEntry)
        {
            foreach (var loggingProxy in _loggingProxies)
            {
                try
                {
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
