using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_ECB.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.ECB
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseValidatorFactory();
        }

        [Test]
        [TestCase("encrypt", "mct", typeof(TestCaseValidatorMCTEncrypt))]
        [TestCase("decrypt", "mct", typeof(TestCaseValidatorMCTDecrypt))]
        [TestCase("encrypt", "somethingThatIsntMct", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "somethingThatIsntMct", typeof(TestCaseValidatorDecrypt))]
        public void ShouldReturnCorrectValidatorType(string direction, string testType, Type expectedType)
        {
            var testVectorSet = GetTestGroup(direction, testType);
            var result = _subject.GetValidators(testVectorSet);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.InstanceOf(expectedType));
        }

        private TestVectorSet GetTestGroup(string direction, string testType)
        {
            TestVectorSet testVectorSet = new TestVectorSet()
            {
                TestGroups = new List<TestGroup>()
                {
                    new TestGroup()
                    {
                        TestType = testType,
                        Function = direction,
                        Tests = new List<TestCase>()
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
