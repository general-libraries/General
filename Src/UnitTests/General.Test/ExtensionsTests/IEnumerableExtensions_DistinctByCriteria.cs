using System;
using NUnit.Framework;
using System.Collections.Generic;
using General.Extensions;
using System.Linq;

namespace General.Test
{

    public class TestObject
    {
        public string Id { get; set; }
        public DateTime MyDate { get; set; }
    }

    [TestFixture]
    public class IEnumerableExtensions_DistinctByCriteria
    {
        
        TestObject _object1;
        TestObject _object2;
        TestObject _object3;

        List<TestObject> _testObjectList;


        [SetUp]
        public void SetUp()
        {
            _object1 = new TestObject { Id = "200", MyDate = new DateTime(2016, 2, 2) }; //Feb 2 2016
            _object2 = new TestObject { Id = "200", MyDate = new DateTime(2016, 1, 2) }; //jan 2 2016
            _object3 = new TestObject { Id="300", MyDate = new DateTime(2016,1,1)}; //jan 1 2016

            _testObjectList = new List<TestObject> { _object1, _object2, _object3 };
        }


        [Test]
        public void aVerifyCorrectValuesReturned()
        {
            var uniqueObjects = _testObjectList.DistinctByCriteria(t => t.Id, _testObjectList => _testObjectList.OrderByDescending(testObject => testObject.MyDate).First());
            Assert.AreEqual(2, uniqueObjects.Count());

        }
    }
}
