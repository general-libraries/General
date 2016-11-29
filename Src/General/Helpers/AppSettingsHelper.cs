using System;
using System.Collections.Generic;
using General.Configuration;

namespace General.Helpers
{
    public static class AppSettingsHelper
    {
        /// <summary>
        /// Returns the value of an appSetting (<paramref name="appSettingKey"/>).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appSettingKey"></param>
        /// <returns></returns>
        /// <exception cref="MissingAppSettingException">thrown if <paramref name="appSettingKey"/> cannot be found in the Web.config (or App.config).</exception>
        public static T Get<T>(string appSettingKey)
        {
            object appSettingValue = System.Configuration.ConfigurationManager.AppSettings[appSettingKey];

            if (appSettingValue == null)
            {
                throw new MissingAppSettingException(appSettingKey, appSettingKey);
            }

            return Get<T>(appSettingKey, default(T));
        }

        /// <summary>
        /// Returns the value of an appSetting (<paramref name="appSettingKey"/>) if it exist; if not, returns <paramref name="defaultValue"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appSettingKey">Key of appSetting</param>
        /// <param name="defaultValue">value to return if <paramref name="appSettingKey"/> does not exist in application settings collection.</param>
        /// <returns>The value of an appSetting if it exist; if not, returns <paramref name="defaultValue"/>.</returns>
        public static T Get<T>(string appSettingKey, object defaultValue)
        {
            T result;

            object appSet = System.Configuration.ConfigurationManager.AppSettings[appSettingKey];

            if (appSet == null)
            {
                try
                {
                    result = (T)defaultValue;
                }
                catch (Exception exception)
                {
                    throw new BadAppSettingException(appSettingKey, defaultValue, typeof(T), exception);
                }
            }
            else
            {
                try
                {
                    if (typeof(T).IsEnum)
                    {
                        result = EnumHelper.Parse<T>(appSet);
                    }
                    else
                    {
                        result = (T)Convert.ChangeType(appSet, typeof(T));
                    }
                }
                catch (Exception exception)
                {
                    throw new BadAppSettingException(appSettingKey, appSet.ToString(), typeof(T), exception);
                }
            }

            return result;
        }

        public static T Get<T>(KeyValuePair<string, T> keyValue)
        {
            return Get<T>(keyValue.Key, keyValue.Value);
        }
    }
}
