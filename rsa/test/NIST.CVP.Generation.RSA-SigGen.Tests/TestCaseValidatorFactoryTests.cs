using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;
        
        [SetUp]
        public void SetUp()
        {
            _subject = new TestCaseValidatorFactory(null, null, null);
        }

        [Test]
        [TestCase("gdt", typeof(TestCaseValidatorGDT))]
        [TestCase("junk", typeof(TestCaseValidatorNull))]
        public void ShouldReturnCorrectValidatorTypeDependentOnFunction(string testType, Type expectedType)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, testType);

            var results = _subject.GetValidators(testVectorSet, suppliedResults);

            Assert.AreEqual(1, results.Count(), "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
        }

        private void GetData(ref TestVectorSet testVectorSet, ref List<TestCase> suppliedResults, string testType)
        {
            testVectorSet = new TestVectorSet
            {
                Algorithm = "",
                Mode = "",
                TestGroups = new List<ITestGroup>
                {
                    new TestGroup
                    {
                        TestType = testType,
                        Modulo = 2048,
                        Mode = SignatureSchemes.Pss,
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                        Tests = new List<ITestCase>
                        {
                            new TestCase
                            {
                                TestCaseId = 1,
                                Message = new BitString("ABCD")
                            }
                        }
                    }
                }
            };

            suppliedResults = new List<TestCase>
            {
                new TestCase
                {
                    TestCaseId = 1,
                    Signature = new BitString("ABCD"),
                    Salt = new BitString("ABCD")
                }
            };
        }
    }
}
