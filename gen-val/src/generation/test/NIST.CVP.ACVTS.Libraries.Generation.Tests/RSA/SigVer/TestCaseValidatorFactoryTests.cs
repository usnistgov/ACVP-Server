using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SigVer
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new TestCaseValidatorFactory();
        }

        [Test]
        [TestCase("gdt", typeof(TestCaseValidator))]
        public void ShouldReturnCorrectValidatorTypeDependentOnFunction(string testType, Type expectedType)
        {
            TestVectorSet testVectorSet = null;

            GetData(ref testVectorSet, testType);

            var results = _subject.GetValidators(testVectorSet);

            Assert.That(results.Count(), Is.EqualTo(1), "Expected 1 validator");
            Assert.That(results.First(), Is.InstanceOf(expectedType));
        }

        private void GetData(ref TestVectorSet testVectorSet, string testType)
        {
            testVectorSet = new TestVectorSet
            {
                Algorithm = "",
                Mode = "",
                TestGroups = new List<TestGroup>
                {
                    new TestGroup
                    {
                        TestType = testType,
                        Modulo = 2048,
                        Mode = SignatureSchemes.Pss,
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                        Tests = new List<TestCase>
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
        }
    }
}
