using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseValidatorFactory();
        }

        [Test]
        [TestCase("encrypt", "mct", typeof(TestCaseValidatorMonteCarloEncrypt))]
        [TestCase("decrypt", "mct", typeof(TestCaseValidatorMonteCarloDecrypt))]
        [TestCase("encrypt", "somethingThatIsntMct", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "somethingThatIsntMct", typeof(TestCaseValidatorDecrypt))]
        public void ShouldReturnCorrectValidatorType(string direction, string testType, Type expectedType)
        {
            var testVectorSet = GetTestGroup(direction, testType);
            var result = _subject.GetValidators(testVectorSet);

            Assert.AreEqual(1, result.Count());
            Assert.IsInstanceOf(expectedType, result.First());
        }

        private TestVectorSet GetTestGroup(string direction, string testType)
        {
            TestVectorSet testVectorSet = new TestVectorSet()
            {
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
                        TestType = testType,
                        Function = direction,
                        Tests = new List<ITestCase>()
                        {
                            new TestCase()
                        }
                    }
                }
            };

            return testVectorSet;
        }
    }
}
