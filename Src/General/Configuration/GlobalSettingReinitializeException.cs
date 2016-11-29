using System;

namespace General.Configuration
{
    /// <summary>
    /// Exception if <see cref="GlobalSettingBase"/>.Initialize() method call for the second time.
    /// </summary>
    public class GlobalSettingReinitializeException : Exception, IException
    {
        public GlobalSettingReinitializeException()
            : base(ExceptionMessages.GlobalSettingReinitializeException)
        { }
    }
}
