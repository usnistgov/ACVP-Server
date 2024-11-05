using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v1_0
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
        [TestCase("encrypt", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", typeof(TestCaseValidatorDecrypt))]
        public void ShouldReturnCorrectValidatorType(string direction, Type expectedType)
        {
            var testVectorSet = GetTestGroup(direction);
            var result = _subject.GetValidators(testVectorSet);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.InstanceOf(expectedType));
        }

        private TestVectorSet GetTestGroup(string direction)
        {
            TestVectorSet testVectorSet = new TestVectorSet
            {
                TestGroups = new List<TestGroup>
                {
                    new TestGroup
                    {
                        Direction = direction,
                        Tests = new List<TestCase>
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
