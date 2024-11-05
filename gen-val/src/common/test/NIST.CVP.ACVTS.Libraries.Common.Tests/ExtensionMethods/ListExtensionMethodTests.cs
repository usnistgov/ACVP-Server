using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Common.Tests.ExtensionMethods
{
    [TestFixture, UnitTest]
    public class ListExtensionMethodTests
    {
        #region AddIfNotNullOrEmpty
        [Test]
        public void ShouldNotAddToListIfNull()
        {
            List<string> list = new List<string>();
            string s = null;
            list.AddIfNotNullOrEmpty(s);
            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldNotAddToListIfEmpty()
        {
            List<string> list = new List<string>();
            string s = "";
            list.AddIfNotNullOrEmpty(s);
            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldAddToListIfNotEmpty()
        {
            List<string> list = new List<string>();
            string s = "Not empty";
            list.AddIfNotNullOrEmpty(s);
            Assert.That(list.Count, Is.EqualTo(1));
        }
        #endregion AddIfNotNullOrEmpty

        #region AddIfNotNull

        public class TestObject
        {
            public string Foo { get; set; }
        }

        private static object[] _testListAddIfNotNull = new object[]
        {
            new object[]
            {
                "Null, don't add",
                null,
                false
            },
            new object[]
            {
                "Object w/ values in object",
                new TestObject()
                {
                    Foo = "Bar"
                },
                true
            },
            new object[]
            {
                "Empty object",
                new TestObject(),
                true
            }
        };

        [Test]
        [TestCaseSource(nameof(_testListAddIfNotNull))]
        public void ShouldAddWhenNotNull(string label, TestObject testObject, bool shouldAdd)
        {
            List<TestObject> list = new List<TestObject>();

            list.AddIfNotNull(testObject);

            int expectedCount = shouldAdd ? 1 : 0;

            Assert.That(list.Count, Is.EqualTo(expectedCount));
        }
        #endregion AddIfNotNull

        #region AddRangeIfNotNullOrEmpty
        [Test]
        public void ShouldDemonstrateErrorOnAddingNullListWithoutExtension()
        {
            List<int> list = new List<int>();
            List<int> addList = null;

            Assert.Throws(typeof(ArgumentNullException), () => list.AddRange(addList));
        }

        [Test]
        public void ShouldNotAddRangeToListIfNull()
        {
            List<int> list = new List<int>();
            List<int> addList = null;

            list.AddRangeIfNotNullOrEmpty(addList);

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldNotAddRangeToListIfEmpty()
        {
            List<int> list = new List<int>();
            List<int> addList = new List<int>();

            list.AddRangeIfNotNullOrEmpty(addList);

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldAddRangeToListIfNotEmpty()
        {
            List<int> list = new List<int>();
            List<int> addList = new List<int>()
            {
                1
            };

            list.AddRangeIfNotNullOrEmpty(addList);

            Assert.That(list.Count, Is.EqualTo(1));
        }
        #endregion AddRangeIfNotNullOrEmpty

        #region AddMultiple

        private enum TestEnum
        {
            One,
            Two
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ShouldOnlyAddNTimesWithEnum(int n)
        {
            List<TestEnum> list = new List<TestEnum>();

            list.Add(TestEnum.One);
            list.Add(TestEnum.Two, n);

            Assert.That(list.Count(w => w == TestEnum.Two), Is.EqualTo(n));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(60)]
        [TestCase(100)]
        public void ShouldOnlyAddNTimes(int n)
        {
            List<object> list = new List<object>();

            list.Add(new object(), n);

            Assert.That(list.Count, Is.EqualTo(n));
        }
        #endregion AddMultiple
    }
}
