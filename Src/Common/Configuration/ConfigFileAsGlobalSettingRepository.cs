using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Helpers;

namespace Common.Configuration
{
    public interface IConfigFileAsGlobalSettingRepository : IGlobalSettingRepository { }

    public class ConfigFileAsGlobalSettingRepository : IConfigFileAsGlobalSettingRepository
    {
        /// <summary>
        /// Gets the value showing if <see cref="GlobalSetting"/>.FriendlyCompanyName is expected as the prefix for appSetting's key.
        /// </summary>
        // TODO: It's better to remove this and automatically search for "Key", "CompanyName.Key", "ApplicationName.Key" in the web.config file.
        //public bool ApplicationNameAsKeyPrefix { get; private set; }

        public IDictionary<string, object> Get()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var dictionary = appSettings.Cast<string>().ToDictionary(s => s, s => (object)appSettings[s]);

            UpdateAesKey(dictionary);
            UpdateIdObfuscation(dictionary);
            dictionary.AddOrUpdate(GlobalSettingKey.LOGMANAGER_LOGLEVEL.Key, AppSettingsHelper.Get(GlobalSettingKey.LOGMANAGER_LOGLEVEL));
            dictionary.AddOrUpdate(GlobalSettingKey.MEMORYCACHE_ABSOLUTE_EXPIRATION.Key, AppSettingsHelper.Get(GlobalSettingKey.MEMORYCACHE_ABSOLUTE_EXPIRATION));
            dictionary.AddOrUpdate(GlobalSettingKey.RECAPTCHA2_ENABLE.Key, AppSettingsHelper.Get(GlobalSettingKey.RECAPTCHA2_ENABLE));
            UpdateRecaptcha2Url(dictionary);
            UpdateVersion(dictionary);

            return dictionary;
        }

        private static void UpdateAesKey(IDictionary<string, object> dictionary)
        {
            string aesKeyString = AppSettingsHelper.Get(GlobalSettingKey.CRYPTOGRAPHY_AES_KEY);

            if (string.IsNullOrEmpty(aesKeyString))
            {
                return;
                //throw new MissingAppSettingException("Cryptography AES Key", GlobalSettingKey.CRYPTOGRAPHY_AES_KEY.Key);
            }

            byte[] aesKey;

            try
            {
                aesKey = StringHelper.StringToByteArray(aesKeyString, ',');
            }
            catch (Exception exception)
            {
                throw new Exception(
                    $"Cannot convert the following comma separated string to byte array. Tip: numbers should be between 0 and 255. \n{aesKeyString}",
                    exception);
            }

            if (aesKey.Length != 32)
            {
                throw new Exception(
                    $"{GlobalSettingKey.CRYPTOGRAPHY_AES_KEY.Key} has an invalid lenght (= {aesKey.Length}) (32 bytes are expected). \nOriginal value: \n{aesKeyString}");
            }

            dictionary.AddOrUpdate(GlobalSettingKey.CRYPTOGRAPHY_AES_KEY.Key, aesKey);
        }

        private void UpdateIdObfuscation(IDictionary<string, object> dictionary)
        {
            bool? idObfuscation = AppSettingsHelper.Get(GlobalSettingKey.ID_OBFUSCATION);

            if (!idObfuscation.HasValue)
                return;

            dictionary.AddOrUpdate(GlobalSettingKey.ID_OBFUSCATION.Key, idObfuscation.Value);
        }

        private static void UpdateRecaptcha2Url(IDictionary<string, object> dictionary)
        {
            string url = AppSettingsHelper.Get(GlobalSettingKey.RECAPTCHA2_URL);

            if(string.IsNullOrWhiteSpace(url))
                return;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new BadAppSettingException(GlobalSettingKey.RECAPTCHA2_URL.Key, url, typeof(Uri));

            dictionary.AddOrUpdate(GlobalSettingKey.RECAPTCHA2_URL.Key, new Uri(url));
        }

        private static void UpdateVersion(IDictionary<string, object> dictionary)
        {
            string ver = AppSettingsHelper.Get(GlobalSettingKey.APPLICATION_VERSION);

            if (string.IsNullOrWhiteSpace(ver))
            {
                return;
            }

            dictionary.AddOrUpdate(GlobalSettingKey.APPLICATION_VERSION.Key, new Version(ver));
        }

        private string FormatKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return key;

            return key.Replace(".", String.Empty).Replace("_", string.Empty);
        }


        //public string FriendlyApplicationName
        //{
        //    get { return _friendlyApplicationName ?? (_friendlyApplicationName = ApplicationName.Replace(" ", string.Empty)); }
        //}
        //protected string _friendlyApplicationName;

        protected string GenerateKeyWithPrefix(string key)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentException("key"); }

            //if (!ApplicationNameAsKeyPrefix || key.StartsWith(FriendlyApplicationName))
            //{
            //    return key;
            //}

            //return $"{FriendlyApplicationName}.{key}";
        }
    }
}
