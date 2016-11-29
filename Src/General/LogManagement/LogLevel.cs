using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.LogManagement
{
    /// <summary>
    /// Level of a log entry.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debugg Level Logging
        /// </summary>
        Debug = 1,
        /// <summary>
        /// Service Level Logging
        /// </summary>
        Trace = 2,
        /// <summary>
        /// Warning Level Logging
        /// </summary>
        Warning = 3,
        /// <summary>
        /// Error Level Logging
        /// </summary>
        Info = 4,
        /// <summary>
        /// Page counter enumerator
        /// </summary>
        Error = 5,
        /// <summary>
        /// Fatal Level Logging
        /// </summary>
        Fatal = 6,
        /// <summary>
        /// Informational Level Logging
        /// </summary>
    }
}
