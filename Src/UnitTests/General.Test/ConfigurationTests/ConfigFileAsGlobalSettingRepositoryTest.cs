using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using General.Configuration;
using NUnit.Framework;

namespace General.Test.ConfigurationTests
{
    [TestFixture]
    public class ConfigFileAsGlobalSettingRepositoryTest
    {
        [Test]
        public void Test001()
        {
            var repo = new ConfigFileAsGlobalSettingRepository();

            var actual = repo.Get();

            Assert.NotNull(actual);
        }

        [Test]
        public void Test002()
        {
            var commonServiceProvider = new CommonServiceProvider();

            commonServiceProvider.AddOrReplace<Object>("TEST OBJECT");

            var actual = commonServiceProvider.GetService<Object>();

            Assert.AreEqual("TEST OBJECT", actual);
        }

        [Test]
        public void Test003()
        {
            var commonServiceProvider = new CommonServiceProvider();

            var actual = commonServiceProvider.GetService<IAssemblyAsGlobalSettingRepository>();

            Assert.AreEqual(typeof(AssemblyAsGlobalSettingRepository), actual.GetType());
        }

    }
}
