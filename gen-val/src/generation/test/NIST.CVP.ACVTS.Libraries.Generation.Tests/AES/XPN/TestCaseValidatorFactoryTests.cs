using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XPN
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseValidatorFactory(null);
        }

        [Test]
        [TestCase("encrypt", false, typeof(TestCaseValidatorExternalEncrypt))]
        [TestCase("encrypt", true, typeof(TestCaseValidatorInternalEncrypt))]
        [TestCase("decrypt", false, typeof(TestCaseValidatorDecrypt))]
        [TestCase("decrypt", true, typeof(TestCaseValidatorDecrypt))]
        public void ShouldReturnCorrectValidatorTypeDependantOnFunction(string function, bool isDeferred, Type expectedType)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, function, isDeferred);

            var results = _subject.GetValidators(testVectorSet);

            Assert.IsTrue(results.Count() == 1, "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
        }

        private void GetData(ref TestVectorSet testVectorSet, ref List<TestCase> suppliedResults, string function, bool isDeferred)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<TestGroup>()
                {
                    new TestGroup()
                    {
                        Function = function,
                        AadLength = 128,
                        TestType = string.Empty,
                        IvGeneration = string.Empty,
                        IvGenerationMode = string.Empty,
                        KeyLength = 128,
                        PayloadLength = 128,
                        TagLength = 128,
                        Tests = new List<TestCase>()
                        {
                            new TestCase()
                            {
                                AAD = new BitString(128),
                                CipherText = new BitString(128),
                                Deferred = isDeferred,
                                TestPassed = true,
                                Key = new BitString(128),
                                PlainText = new BitString(128),
                                Tag = new BitString(128),
                                TestCaseId = 1
                            }
                        }
                    }
                }
            };

            suppliedResults = new List<TestCase>()
            {
                new TestCase()
                {
                    IV = new BitString(128),
                    TestCaseId = 1
                }
            };
        }
    }
}
