using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.LogManagement
{
    public interface ILoggingProxy
    {
        bool IsInitialized { get; }

        bool Initialize(string applicationName, string applicationVersion);

        IQueryable<LogEntry> Get(
            int startRowIndex = 0,
            int maximumRows = 100,
            LogLevel[] logLevels = null,
            string message = null,
            string errorMessage = null,
            string applicationName = null,
            string applicationId = null,
            string methodName = null,
            string className = null,
            int? ManagedThreadId = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        );

        void Save(LogEntry logEntity);

        void Remove(DateTime olderThanDate);

        bool Test();
    }
}

