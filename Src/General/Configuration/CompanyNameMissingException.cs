using System;

namespace General.Configuration
{
    public class CompanyNameMissingException : Exception, IException
    {
        public CompanyNameMissingException()
            : base(string.Format(ExceptionMessages.CompanyNameMissingException, GlobalSettingKey.COMPANY_NAME))
        { }
    }
}
