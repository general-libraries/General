using System;
using System.Collections.Generic;
using Common.Extensions;

namespace Common.Configuration
{
    public class GlobalSettingProvider : IGlobalSettingProvider
    {
        protected readonly IAssemblyAsGlobalSettingRepository AssemblyRepo;
        protected readonly IConfigFileAsGlobalSettingRepository ConfigFileRepo;
        protected readonly IGlobalSettingRepository SettingRepo;

        public GlobalSettingProvider(
            IAssemblyAsGlobalSettingRepository assemblyAsGlobalSettingRepository,
            IConfigFileAsGlobalSettingRepository configFileAsGlobalSettingRepository,
            IGlobalSettingRepository globalSettingRepository)
        {
            AssemblyRepo = assemblyAsGlobalSettingRepository;
            ConfigFileRepo = configFileAsGlobalSettingRepository;
            SettingRepo = globalSettingRepository;
        }

        public virtual GlobalSetting Get()
        {
            var aggregatedDic = new Dictionary<string, object>();

            // TODO: use cache here!!!

            if (AssemblyRepo != null)
            {
                aggregatedDic.AddOrUpdate(AssemblyRepo.Get());
            }

            if (ConfigFileRepo != null)
            {
                aggregatedDic.AddOrUpdate(ConfigFileRepo.Get());
            }

            if (SettingRepo != null)
            {
                aggregatedDic.AddOrUpdate(SettingRepo.Get());
            }

            return new GlobalSetting(aggregatedDic);
        }
    }
}
