using System;
using System.CodeDom;
using Common.Helpers;
using NUnit.Framework;

namespace Common.Test.HelpersTests
{
    [TestFixture]
    public class ConvertHelperTests
    {
        [TestCase(null, 0)]
        [TestCase(" 12  8 ", 128)]
        [TestCase(" 12.8 ", 13)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase("79228162514264337593543950335M", 0)] // decimal.MaxValue
        [TestCase("79228162514264337593543950335", 0)] // decimal.MaxValue
        [TestCase(79228162514.259, 0)] //out of int range
        [TestCase("2147483647.49", 2147483647)] // max int 
        [TestCase("2147483647.50", 0)] // out of int range
        [TestCase("-2147483648.49", -2147483648)] // min int
        public void ToInt32_Test(object input, int expected)
        {
            var actual = ConvertHelper.ToInt32(input);
            
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, 0)]
        [TestCase(" 12  8 ", 128)]
        [TestCase(" 12.8 ", 12.8)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(int.MinValue, int.MinValue)]
        public void ToDecimal_Test(object input, decimal expected)
        {
            decimal actual = ConvertHelper.ToDecimal(input);

            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void ToDecimal_Test002()
        {
            decimal expected = 79228162514.25956M;

            decimal actual = ConvertHelper.ToDecimal("79228162514.25956");

            Assert.AreEqual(expected, actual);
        }
    }
}
