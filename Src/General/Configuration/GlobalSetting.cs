using System;
using System.Collections.Generic;
using General.LogManagement;

namespace General.Configuration
{
    /// <summary>
    /// Encapsulates static properties to prived global application settings.
    /// </summary>
    /// <remarks>
    /// For initializing members of this class, call <see cref="GlobalSetting"/>.Initialize() method at the starting point of the application (such as Application_Start() method inside Global.asax.cs file).
    /// </remarks>
    public class GlobalSetting
    {
        //protected readonly IDictionary<string, object> _settingDictionary;

        public GlobalSetting(IDictionary<string, object> settingDictionary)
        {
            if (settingDictionary == null)
            {
                throw new ArgumentNullException("settingDictionary");
            }

            CompanyName = GetFrom<string>(settingDictionary, GlobalSettingKey.COMPANY_NAME.Key);
            ApplicationName = GetFrom<string>(settingDictionary, GlobalSettingKey.APPLICATION_NAME.Key);

            if (settingDictionary.ContainsKey(GlobalSettingKey.CRYPTOGRAPHY_AES_KEY.Key))
            {
                _aesKey = GetFrom<byte[]>(settingDictionary, GlobalSettingKey.CRYPTOGRAPHY_AES_KEY.Key);
            }

            Version = GetFrom<Version>(settingDictionary, GlobalSettingKey.APPLICATION_VERSION.Key);
            LogLevel = GetFrom<LogLevel>(settingDictionary, GlobalSettingKey.LOGMANAGER_LOGLEVEL.Key);
            Recaptcha2Enable = GetFrom<bool>(settingDictionary, GlobalSettingKey.RECAPTCHA2_ENABLE.Key);
            Recaptcha2Url = GetFrom<Uri>(settingDictionary, GlobalSettingKey.RECAPTCHA2_URL.Key);
            Recaptcha2PublicKey = GetFrom<string>(settingDictionary, GlobalSettingKey.RECAPTCHA2_PUBLIC_KEY.Key);
            Recaptcha2PrivateKey = GetFrom<string>(settingDictionary, GlobalSettingKey.RECAPTCHA2_PRIVATE_KEY.Key);
            GeneralSalt = GetFrom<string>(settingDictionary, GlobalSettingKey.GENERAL_SALT.Key);
            MemoryCacheAbsoluteExpiration = GetFrom<double>(settingDictionary, GlobalSettingKey.MEMORYCACHE_ABSOLUTE_EXPIRATION.Key);
            IdObfuscation = GetFrom<bool>(settingDictionary, GlobalSettingKey.ID_OBFUSCATION.Key);
            ;
        }

        /// <summary>
        /// Initializes <see cref="GlobalSetting"/>. Must be called at the start of the application (Call it inside Global.asax's Application_Start() methode).
        /// </summary>
        /// <param name="applicationNameAsKeyPrefix">Use <see cref="GlobalSetting"/>.FriendlyApplicationName as the prefix for appSetting's key.</param>
        /// <param name="appSettingKeys">List of appSetting keys that must be defined in Web.config (or App.config).</param>
        /// <exception cref="CompanyNameMissingException"></exception>
        //public static void Initialize(bool applicationNameAsKeyPrefix = false, IEnumerable<string> appSettingKeys = null)
        //{
        //    var callingAssembly = System.Reflection.Assembly.GetCallingAssembly();
        //    //SetCompanyName(GlobalSettingKey.COMPANY_NAME, callingAssembly);
        //    //SetApplicationName(GlobalSettingKey.APPLICATION_NAME, callingAssembly);
        //    //SetApplicationVersion(GlobalSettingKey.APPLICATION_VERSION, callingAssembly);

        //    if (appSettingKeys != null)
        //    {

        //        foreach (var key in appSettingKeys)
        //        {
        //            if (System.Configuration.ConfigurationManager.AppSettings[key] == null)
        //            {
        //                throw new MissingAppSettingException(key, key);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Gets the name of your company.
        /// </summary>
        /// <value>The name of your company.</value>
        /// <remarks>
        /// CompanyName can be set in 2 ways:
        ///   1- Adding "CompanyName" key/value to the {appSettings} section of Web.config or App.config of your application;
        ///   2- Setting [assembly: AssemblyCompany("Company Name")] in AssemblyInfo.cs.
        /// </remarks>
        public string CompanyName { get; private set; }

        public string FriendlyCmpanyName
        {
            get { return _friendlyCompanyName ?? (_friendlyCompanyName = GenerateFriendlyCmpanyName(CompanyName)); }
        }
        private string _friendlyCompanyName;

        /// <summary>
        /// Gets the name of the current running application.
        /// </summary>
        /// <value>The name of the current running application.</value>
        public string ApplicationName { get; private set; }

        /// <summary>
        /// Gets version of the current application.
        /// </summary>
        /// <value>
        /// The version of the current application.
        /// </value>
        public Version Version { get; private set; }

        /// <summary>
        /// Gets version of the current application in string.
        /// </summary>
        /// <value>
        /// The version of the current application in string.
        /// </value>
        public string VersionString
        {
            get
            {   //TODO: add the logic to force developer to run Initialize() method before calling this property.
                if (string.IsNullOrEmpty(_versionString) && Version != null)
                {
                    _versionString = $"{Version.Major}.{Version.Minor}.{Version.Build}.{Version.Revision}";
                }

                return _versionString;
            }
        }
        protected string _versionString;

        /// <summary>
        /// Gets the global log level
        /// </summary>
        /// <value>AES Key</value>
        public LogLevel LogLevel { get; private set; }

        /// <summary>
        /// Gets Google Recaptcha base URL.
        /// </summary>
        /// <value>Google Recaptcha base URL</value>
        public bool Recaptcha2Enable { get; private set; }

        /// <summary>
        /// Gets Google Recaptcha base URL.
        /// </summary>
        /// <value>Google Recaptcha base URL</value>
        public Uri Recaptcha2Url { get; private set; }

        /// <summary>
        /// Gets Google Recaptcha Public Key.
        /// </summary>
        /// <value>Google Recaptcha Public Key</value>
        public string Recaptcha2PublicKey { get; private set; }

        /// <summary>
        /// Gets Google Recaptcha Private Key.
        /// </summary>
        /// <value>Google Recaptcha Private Key</value>
        public string Recaptcha2PrivateKey { get; private set; }

        public string GeneralSalt { get; private set; }

        /// <summary>
        /// Gets global number of seconds that for absolute expiration of memory cache.
        /// </summary>
        /// <value>Global number of seconds that for absolute expiration of memory cache.</value>
        /// <remarks>
        /// Developer can override this value for an specific cache or globalle. <see cref="MemoryCacheManager{T}"/>
        /// </remarks>
        public double MemoryCacheAbsoluteExpiration { get; private set; }

        /// <summary>
        /// Gets AES Key for <see cref="Cryptography.AES"/> Cryptography.
        /// </summary>
        /// <value>AES Key</value>
        public byte[] AesKey
        {
            get
            {
                if (_aesKey == null)
                    throw new Exception("no value has been set for AesKey!");
                return _aesKey;
            }
        }
        private readonly byte[] _aesKey;

        public bool IdObfuscation { get; private set; }

        protected T GetFrom<T>(IDictionary<string, object> settingDictionary, string key)
        {
            if (!settingDictionary.ContainsKey(key))
                throw new ArgumentException(string.Format("Error in instantiating GlobalSetting object because '{0}' in the dictionary cannot be found!", key), "dictionary");

            var value = settingDictionary[key];

            if (value is T)
                return (T)value;

            throw new ArgumentException(string.Format("Error in instantiating GlobalSetting object because the type of '{0}' in the dictionary is not desired (expected type: {1})!", key, typeof(T).FullName), "dictionary");
        }

        #region private methods

        private static string GenerateFriendlyCmpanyName(string companyName)
        {
            return
                companyName
                    .Replace("Co.", string.Empty)
                    .Replace("Ltd.", string.Empty)
                    .Replace("Inc.", string.Empty)
                    .Replace(" ", string.Empty);
        }

        #endregion
    }
}
