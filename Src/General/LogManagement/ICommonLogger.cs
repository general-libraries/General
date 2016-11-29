using System;

namespace General.LogManagement
{
    public interface ICommonLogger
    {
        /// <summary>
        /// Logs an event to the log repository.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        /// <param name="className"></param>
        /// <param name="stackFrameIndex"></param>
        void Log(LogLevel logLevel, Exception exception = null, string message = "", string methodName = "", string className = "", int stackFrameIndex = 1);
    }
}
