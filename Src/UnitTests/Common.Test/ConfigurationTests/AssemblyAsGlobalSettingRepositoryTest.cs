using System;
using System.Reflection;
using Common.Configuration;
using NUnit.Framework;

namespace Common.Test.ConfigurationTests
{
    [TestFixture]
    public class AssemblyAsGlobalSettingRepositoryTest
    {
        [Test]
        public void AssemblyAsGlobalSettingRepository_Get_Basic_Test()
        {
            // arrange
            var assembly = Assembly.GetAssembly(this.GetType());
            var settingRepository = new AssemblyAsGlobalSettingRepository(assembly);

            // act
            var result = settingRepository.Get();

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void AssemblyAsGlobalSettingRepository_Get_CompanyName_Test()
        {
            // arrange
            var assembly = Assembly.GetAssembly(this.GetType());
            var assemblyCompanyAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute), false) as AssemblyCompanyAttribute;
            var expected = string.Empty;

            if (assemblyCompanyAttribute != null)
            {
                expected = assemblyCompanyAttribute.Company;
            }

            // act
            var settingRepository = new AssemblyAsGlobalSettingRepository(assembly);
            var settingDic = settingRepository.Get();
            var actual = settingDic[GlobalSettingKey.COMPANY_NAME.Key];

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AssemblyAsGlobalSettingRepository_Get_ApplicationName_Test()
        {
            // arrange
            var assembly = Assembly.GetAssembly(this.GetType());
            var expected = assembly.GetName().Name;

            // act 
            var settingRepository = new AssemblyAsGlobalSettingRepository(assembly);
            var settingDic = settingRepository.Get();
            var actual = settingDic[GlobalSettingKey.APPLICATION_NAME.Key];

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AssemblyAsGlobalSettingRepository_Get_Version_Test()
        {
            // arrange
            var assembly = Assembly.GetAssembly(this.GetType());
            var expected = assembly.GetName().Version;

            // act 
            var settingRepository = new AssemblyAsGlobalSettingRepository(assembly);
            var settingDic = settingRepository.Get();
            var actual = settingDic[GlobalSettingKey.APPLICATION_VERSION.Key];

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AssemblyAsGlobalSettingRepository_Get_Basic_Null_Assembly_Test()
        {
            // arrange
            var settingRepository = new AssemblyAsGlobalSettingRepository(null);

            // act
            var settings = settingRepository.Get();

            // assert
            Assert.NotNull(settings);
            Assert.AreEqual(3, settings.Count);
            Assert.AreEqual("Common.Test", settings[GlobalSettingKey.APPLICATION_NAME.Key]);
        }

    }
}
