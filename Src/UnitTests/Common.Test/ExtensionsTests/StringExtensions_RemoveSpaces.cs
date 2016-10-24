using NUnit.Framework;

using Common.Extensions;





namespace Common.Test.Extension_Tests

{

    [TestFixture]

    class StringExtensions_RemoveSpaces

    {

        [TestCase("one one")]

        [TestCase("oneone   ")]

        [TestCase("  one   one")]

        [TestCase(" o   ne      on e   ")]

        [TestCase("oneone")]

       public void TestDifferentScenarios(string input)

        {

            var result = input.RemoveSpaces();

            Assert.IsTrue(result.Equals("oneone"));

        }







    }

}

