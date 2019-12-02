using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.AES_FF.Tests
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

            Assert.AreEqual(1, result.Count());
            Assert.IsInstanceOf(expectedType, result.First());
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
