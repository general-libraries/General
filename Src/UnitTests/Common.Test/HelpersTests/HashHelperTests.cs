using System;
using Common.Helpers;
using NUnit.Framework;

namespace Common.Test.HelpersTests
{
    [TestFixture]
    public class HashHelperTests
    {
        [TestCase(0L)]
        [TestCase(99L)]
        [TestCase(1500L)]
        public void TestMethod1(long longNumber)
        {
            var encoded = HashHelper.Encode(longNumber);
            var decoded = HashHelper.Decode(encoded, typeof(Int64));

            Assert.AreEqual(longNumber, decoded);
        }
    }
}
