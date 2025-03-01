﻿using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CTR
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
        [TestCase("encrypt", "singleblock", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "SingleBlock", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "partialblock", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "PartialBlock", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "GFSBOX", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "gfsbox", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "KeySBox", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "KEYSBOX", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "VarKey", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "varkey", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "Vartxt", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "varTXT", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypT", "ctr", typeof(TestCaseValidatorCounterEncrypt))]
        [TestCase("DECRYPT", "CtR", typeof(TestCaseValidatorCounterDecrypt))]
        [TestCase("encrypt", "Junk", typeof(TestCaseValidatorNull))]
        [TestCase("", "", typeof(TestCaseValidatorNull))]
        public void ShouldReturnCorrectValidatorType(string direction, string testType, Type expectedType)
        {
            var testVectorSet = GetTestGroup(direction, testType);
            var result = _subject.GetValidators(testVectorSet);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.InstanceOf(expectedType));
        }

        private TestVectorSet GetTestGroup(string direction, string testType)
        {
            var testVectorSet = new TestVectorSet
            {
                TestGroups = new List<TestGroup>
                {
                    new TestGroup
                    {
                        InternalTestType = testType,
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
