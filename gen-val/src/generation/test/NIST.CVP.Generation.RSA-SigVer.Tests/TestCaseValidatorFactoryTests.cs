﻿using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.RSA.v1_0.SigVer;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
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

            Assert.AreEqual(1, results.Count(), "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
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