using System.Collections.Generic;
using Common.LogManagement;

namespace Common.Configuration
{
    public static class GlobalSettingKey
    {
        public static readonly KeyValuePair<string, string> COMPANY_NAME
            = new KeyValuePair<string, string>("CompanyName", null);

        public static readonly KeyValuePair<string, string> APPLICATION_NAME
            = new KeyValuePair<string, string>("ApplicationName", string.Empty);

        public static readonly KeyValuePair<string, string> APPLICATION_VERSION
            = new KeyValuePair<string, string>("Version", null);


        public static readonly KeyValuePair<string, LogLevel> LOGMANAGER_LOGLEVEL
            = new KeyValuePair<string, LogLevel>("LogManager.LogLevel", LogLevel.Debug);

        public static readonly KeyValuePair<string, string> CRYPTOGRAPHY_AES_KEY
            = new KeyValuePair<string, string>("Cryptography.AES.Key", null);


        public static readonly KeyValuePair<string, bool> RECAPTCHA2_ENABLE
            = new KeyValuePair<string, bool>("Recaptcha2.Enable", true);

        public static readonly KeyValuePair<string, string> RECAPTCHA2_URL
            = new KeyValuePair<string, string>("Recaptcha2.Url", null);

        public static readonly KeyValuePair<string, string> RECAPTCHA2_PUBLIC_KEY
            = new KeyValuePair<string, string>("Recaptcha2.PublicKey", null);

        public static readonly KeyValuePair<string, string> RECAPTCHA2_PRIVATE_KEY
            = new KeyValuePair<string, string>("Recaptcha2.PrivateKey", null);


        public static readonly KeyValuePair<string, double> MEMORYCACHE_ABSOLUTE_EXPIRATION
            = new KeyValuePair<string, double>("MemoryCache.AbsoluteExpirationInSeconds", 0d);


        public static readonly KeyValuePair<string, string> GENERAL_SALT
            = new KeyValuePair<string, string>("GeneralSalt", null);

        public static readonly KeyValuePair<string, bool?> ID_OBFUSCATION
            = new KeyValuePair<string, bool?>("IdObfuscation", true);
    }
}
