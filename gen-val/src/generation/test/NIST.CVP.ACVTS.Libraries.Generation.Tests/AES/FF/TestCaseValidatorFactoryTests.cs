using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.FF
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
        [TestCase(BlockCipherDirections.Encrypt, "somethingThatIsntMct", typeof(TestCaseValidatorEncrypt))]
        [TestCase(BlockCipherDirections.Decrypt, "somethingThatIsntMct", typeof(TestCaseValidatorDecrypt))]
        public void ShouldReturnCorrectValidatorType(BlockCipherDirections direction, string testType, Type expectedType)
        {
            var testVectorSet = GetTestGroup(direction, testType);
            var result = _subject.GetValidators(testVectorSet);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.InstanceOf(expectedType));
        }

        private TestVectorSet GetTestGroup(BlockCipherDirections direction, string testType)
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
