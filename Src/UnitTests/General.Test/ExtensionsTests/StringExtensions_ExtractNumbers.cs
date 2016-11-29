using NUnit.Framework;
using General.Extensions;

namespace General.Test.ExtensionsTests
{
    [TestFixture]
    class StringExtensions_ExtractNumbers
    {
        [TestCase("(732) 524-4174")]
        [TestCase("732-524-4174")]
        [TestCase("732 524 4174")]
        [TestCase("732   524   4174")]
        [TestCase("732.524.4174")]
        [TestCase("732.524.4174")]
        [TestCase("732/524/4174")]
        [TestCase("732abd524qwewq@4174")]
        [TestCase("7325244174")]
        [TestCase("  732  524  4174  ")]
        public void When_Numbers_Present(string input)
        {
            var result = input.ExtractNumbers();

            Assert.IsTrue(result.Equals("7325244174"));
        }

        [TestCase("(XXX) YYY-ZZZZ")]
        [TestCase("abcdefghi")]
        [TestCase("")]
        [TestCase(null)]
        public void When_Numbers_Are_Not_Present(string input)
        {
            var result = input.ExtractNumbers();

            Assert.IsTrue(result.Equals(string.Empty));
        }
    }
}
