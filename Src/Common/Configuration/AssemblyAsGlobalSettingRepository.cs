using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Configuration
{
    public interface IAssemblyAsGlobalSettingRepository : IGlobalSettingRepository { }

    /// <summary>
    /// Implementation for <see cref="IGlobalSettingRepository"/> to get some of the Globall Settings from an assembly.
    /// </summary>
    public class AssemblyAsGlobalSettingRepository : IAssemblyAsGlobalSettingRepository
    {
        private Assembly _assembly;

        public AssemblyAsGlobalSettingRepository(Assembly assembly)
        {
            _assembly = assembly ?? Assembly.GetCallingAssembly();
        }

        public IDictionary<string, object> Get()
        {
            if (_assembly == null)
                _assembly = Assembly.GetCallingAssembly();

            IDictionary<string, object> result = new Dictionary<string, object>();

            result.Add(GlobalSettingKey.COMPANY_NAME.Key, GetAssemblyCompany(_assembly));
            result.Add(GlobalSettingKey.APPLICATION_NAME.Key, _assembly.GetName().Name);
            result.Add(GlobalSettingKey.APPLICATION_VERSION.Key, _assembly.GetName().Version);

            return result;
        }

        private static string GetAssemblyCompany(Assembly assembly)
        {
            var assemblyCompanyAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute), false) as AssemblyCompanyAttribute;

            if (assemblyCompanyAttribute == null || string.IsNullOrEmpty(assemblyCompanyAttribute.Company))
            {
                return string.Empty;
            }

            return assemblyCompanyAttribute.Company;
        }
    }
}
