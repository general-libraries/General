using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Common.LogManagement
{
    /// <summary>
    /// Encapsoluate a log entry members.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Gets Unique ID of the logEntry
        /// </summary>
        public long LogID { get; protected set; }

        /// <summary>
        /// Severity level of the log.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the date the log is created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets text message of the log.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets ExceptionData, if an exception causes this log.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the Stack Trace.
        /// </summary>
        public StackTrace StackTrace { get; set; }

        /// <summary>
        /// Gets or sets Method name, from there this log entry was created.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets Class name, from there this log entry was created.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets Application name, from there this log entry was created.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets version of the application/assembly.
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets name of the coputer or server.
        /// </summary>
        public String MachineName { get; set; }

        /// <summary>
        /// Gets or sets Windows Identity.
        /// </summary>
        public string WindowsIdentity { get; set; }

        /// <summary>
        /// Gets or sets Thread Identity.
        /// </summary>
        public string ThreadIdentity { get; set; }

        /// <summary>
        /// Gets or sets Process ID.
        /// </summary>
        public int ProcessID { get; set; }

        /// <summary>
        /// Gets or sets Stack Frame.
        /// </summary>
        public StackFrame StackFrame { get; set; }

        /// <summary>
        /// Gets or sets ThreadData, containing part of Thread data when this log was created.
        /// </summary>
        public ThreadData Thread { get; set; }

        /// <summary>
        /// Default constructor of class.
        /// </summary>
        public LogEntry()
        {
            this.Thread = new ThreadData();
            this.Date = DateTime.Now;
        }

        /// <summary>
        /// Returns a string that represents the current instance of <see cref="LogEntry"/>.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0}  |  {1}  |  App: {2}", this.Date.ToString(), this.Level.ToString(), this.ApplicationName);
            if (!string.IsNullOrEmpty(this.Message))
            {
                stringBuilder.AppendFormat("  |  Message: {0}", this.Message);
            }
            if (!string.IsNullOrEmpty(this.ClassName))
            {
                stringBuilder.AppendFormat("  |  Class: {0}", this.ClassName);
            }
            if (!string.IsNullOrEmpty(this.MethodName))
            {
                stringBuilder.AppendFormat("  |  Method: {0}", this.MethodName);
            }
            stringBuilder.AppendFormat("  |  Thread ID: {0}", this.Thread.ManagedThreadId.ToString());

            if (this.Exception != null)
            {
                stringBuilder.AppendFormat("  |  Exception: {0}", this.Exception);
                if (!string.IsNullOrWhiteSpace(this.Exception.Message))
                {
                    stringBuilder.AppendFormat("  -  Message: {0}", this.Exception.Message);
                }
                if (this.Exception.InnerException != null)
                {
                    stringBuilder.AppendFormat("  -  InnerException: {0}", this.Exception.InnerException);
                }
                if (!string.IsNullOrEmpty(this.Exception.Source))
                {
                    stringBuilder.AppendFormat("  -  Source = {0}", this.Exception.Source);
                }
                if (!string.IsNullOrEmpty(this.Exception.StackTrace))
                {
                    stringBuilder.AppendFormat("  -  StackTrace = {0}", this.Exception.StackTrace);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encapsulates some of a Thread data.
        /// </summary>
        public class ThreadData
        {
            /// <summary>
            /// Gets or sets Thread ID of the Thread that this ThreadData is created based on.
            /// </summary>
            public int ManagedThreadId { get; set; }

            /// <summary>
            /// Gets or sets Thread Name of the Thread that this ThreadData is created based on.
            /// </summary>
            public string ThreadName { get; set; }

            /// <summary>
            /// Gets or sets the culture of the Thread that this ThreadData is created based on.
            /// </summary>
            public string CurrentCulture { get; set; }

            /// <summary>
            /// Gets or sets UI Culture of the Thread that this ThreadData is created based on.
            /// </summary>
            public string CurrentUICulture { get; set; }

            /// <summary>
            /// Gets or sets Priority of the Thread that this ThreadData is created based on.
            /// </summary>
            public ThreadPriority Priority { get; set; }

            /// <summary>
            /// Gets or sets state of the Thread that this ThreadData is created based on.
            /// </summary>
            public System.Threading.ThreadState ThreadState { get; set; }

            public bool IsBackground { get; set; }

            public bool IsThreadPoolThread { get; set; }

            /// <summary>
            /// Constructor of class.
            /// </summary>
            public ThreadData()
            {
                System.Threading.Thread thread = System.Threading.Thread.CurrentThread;
                this.ManagedThreadId = thread.ManagedThreadId;
                this.ThreadName = thread.Name ?? string.Empty;
                this.CurrentCulture = thread.CurrentCulture.EnglishName;
                this.CurrentUICulture = thread.CurrentUICulture.EnglishName;
                this.Priority = thread.Priority;
                this.ThreadState = thread.ThreadState;
                this.IsBackground = thread.IsBackground;
                this.IsThreadPoolThread = thread.IsThreadPoolThread;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (this == null && obj == null)
                {
                    return true;
                }

                ThreadData threadData = obj as ThreadData;

                if (threadData == null)
                {
                    return false;
                }

                return this.CurrentCulture == threadData.CurrentCulture
                    && this.CurrentUICulture == threadData.CurrentUICulture
                    && this.ManagedThreadId == threadData.ManagedThreadId
                    && this.Priority == threadData.Priority
                    && this.ThreadName == threadData.ThreadName
                    && this.ThreadState == threadData.ThreadState;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return CurrentCulture.GetHashCode() * CurrentUICulture.GetHashCode() * ManagedThreadId.GetHashCode()
                            * Priority.GetHashCode() * ThreadName.GetHashCode() * ThreadState.GetHashCode()
                            * IsBackground.GetHashCode() * IsThreadPoolThread.GetHashCode();
                }
            }

            public override string ToString()
            {
                var sBuilder = new StringBuilder();
                sBuilder.AppendFormat("ManagedThreadId = {0} | ", this.ManagedThreadId)
                        .AppendFormat("ThreadName = {0} | ", this.ThreadName)
                        .AppendFormat("CurrentCulture = {0} | ", this.CurrentCulture)
                        .AppendFormat("CurrentUICulture = {0} | ", this.CurrentUICulture)
                        .AppendFormat("Priority = {0} | ", this.Priority)
                        .AppendFormat("ThreadState = {0} | ", this.ThreadState.ToString())
                        .AppendFormat("IsBackground = {0} | ", this.IsBackground)
                        .AppendFormat("IsThreadPoolThread = {0} | ", this.IsThreadPoolThread);
                return sBuilder.ToString();
            }

            public string ToStringXml()
            {
                var sBuilder = new StringBuilder("<thread>");
                sBuilder.AppendFormat("<managedThreadId>{0}</managedThreadId>", this.ManagedThreadId)
                        .AppendFormat("<threadName>{0}</threadName>", this.ThreadName)
                        .AppendFormat("<currentCulture>{0}</currentCulture>", this.CurrentCulture)
                        .AppendFormat("<currentUICulture>{0}</currentUICulture>", this.CurrentUICulture)
                        .AppendFormat("<priority>{0}</priority>", this.Priority)
                        .AppendFormat("<threadState>{0}</threadState>", this.ThreadState.ToString())
                        .AppendFormat("<isBackground>{0}</isBackground>", this.IsBackground)
                        .AppendFormat("<isThreadPoolThread>{0}</isThreadPoolThread>", this.IsThreadPoolThread)
                        .Append("</thread>");
                return sBuilder.ToString();
            }
        }
    }
}
