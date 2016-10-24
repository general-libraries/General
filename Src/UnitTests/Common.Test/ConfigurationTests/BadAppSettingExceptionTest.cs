using System;
using Common.Configuration;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Common.Test.ConfigurationTests
{
    [TestFixture]
    public class BadAppSettingExceptionTest
    {
        [TestCase(null, null, null)]
        [TestCase("key", "value", typeof(string))]
        public void BadAppSettingException_Constructor(
            string settingKeyName, object value, Type expectedType)
        {
            // arrange
            // parameters

            // act
            var actual = new BadAppSettingException(settingKeyName, value, expectedType);

            // assert
            Assert.AreEqual(settingKeyName, actual.Key);
            Assert.AreEqual(value, actual.Value);
            Assert.AreEqual(expectedType, actual.ExpectedType);
            Assert.AreEqual(null, actual.InnerException);
        }

        [Test]
        public void BadAppSettingException_Constructor_with_InnerException()
        {
            // arrange
            const string settingKeyName = null;
            const object value = null;
            Type expectedType = null;
            Exception innrException = new Exception();

            // act
            var actual = new BadAppSettingException(settingKeyName, value, expectedType, innrException);

            // assert
            Assert.AreEqual(innrException, actual.InnerException);
        }
    }
}
