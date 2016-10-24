using System;

namespace Common.Configuration
{
    /// <summary>
    /// Exception for a missing appSetting.
    /// </summary>
    public class MissingAppSettingException : Exception, IException
    {
        /// <summary>
        /// Constructor of MissingAppSettingException. 
        /// </summary>
        /// <param name="missingElement">Title of the missing element. E.g. "Application Name".</param>
        /// <param name="key">key that is missing from Web.config or App.config.</param>
        public MissingAppSettingException(string missingElement, string key)
            : base(string.Format(ExceptionMessages.MissingAppSettingException, missingElement, key))
        { }
    }
}
