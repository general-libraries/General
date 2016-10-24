using System;

namespace Common.Configuration
{
    /// <summary>
    /// Exception when the value of an appSetting in Web.config (or App.config) cannot be converted to a desired type.
    /// </summary>
    public class BadAppSettingException : Exception, IException
    {
        /// <summary>
        /// Gets the key for the application setting.
        /// </summary>
        public string Key { get; protected set; }

        /// <summary>
        /// Gets the value of the application setting.
        /// </summary>
        public object Value { get; protected set; }

        /// <summary>
        /// Gets the expected type for the application setting.
        /// </summary>
        public Type ExpectedType { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BadAppSettingException"/>.
        /// </summary>
        /// <param name="applicationSettingKeyName"></param>
        /// <param name="appSettingValue"></param>
        /// <param name="type"></param>
        /// <param name="innerException"></param>
        public BadAppSettingException(string applicationSettingKeyName, object appSettingValue, Type type, Exception innerException = null)
            : base(string.Format(ExceptionMessages.BadAppSettingException, applicationSettingKeyName, appSettingValue,
                type == null ? null : type.FullName), innerException)
        {
            Key = applicationSettingKeyName;
            Value = appSettingValue;
            ExpectedType = type;
        }
    }
}
