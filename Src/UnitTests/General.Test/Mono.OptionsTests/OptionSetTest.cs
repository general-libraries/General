using System.Collections.Generic;
using General.Mono.Options;
using NUnit.Framework;

namespace General.Test.Mono.OptionsTests
{
    [TestFixture]
    class OptionSetTest
    {
        [Test]
        public void OptionSet_Basic_Test()
        {
            var verbose = 0;
            var values = new List<string>();
            var cmdStr = "-v --v /v -name=A /name B extra";
            var cmdStrArr = cmdStr.Split(new char[1] { ' ' });

            var OptionSet = new OptionSet()
                .Add("v", v => ++verbose)
                .Add("name=|value=", v => values.Add(v));

            var actual = OptionSet.Parse(cmdStrArr);

            Assert.AreEqual(3, verbose);
            Assert.True(values.Contains("A"));
            Assert.True(values.Contains("B"));
            Assert.True(actual.Contains("extra"));
        }

        [Test]
        public void OptionSet_minus_plus_Test002()
        {
            string actual = null;
            var OptionSet =
                new OptionSet
                {
                    { "a", s => actual = s },
                };

            OptionSet.Parse(new string[] { "-a", "-b" });   // sets v != null
            Assert.AreEqual("a", actual);

            OptionSet.Parse(new string[] { "-a+", "-b" });  // sets v != null
            Assert.AreEqual("-a+", actual);

            OptionSet.Parse(new string[] { "-a-", "-b" });  // sets v == null
            Assert.IsNull(actual);
        }
    }
}
