using NIST.CVP.Generation.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new TestCaseValidatorFactory();
        }

        [Test]
        [TestCase("mct", typeof(TestCaseValidatorMCTHash))]
        [TestCase("notmct", typeof(TestCaseValidatorHash))]
        public void ShouldReturnCorrectValidatorType(string testType, Type expectedType)
        {
            var testVectorSet = GetTestGroup(testType);
            var result = _subject.GetValidators(testVectorSet, null);

            Assert.AreEqual(1, result.Count());
            Assert.IsInstanceOf(expectedType, result.First());
        }

        private TestVectorSet GetTestGroup(string testType)
        {
            return new TestVectorSet()
            {
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
                        TestType = testType,
                        Tests = new List<ITestCase>()
                        {
                            new TestCase()
                        }
                    }
                }
            };
        }
    }
}
