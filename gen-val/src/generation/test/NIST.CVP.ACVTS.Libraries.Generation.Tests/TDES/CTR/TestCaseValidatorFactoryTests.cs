﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CTR
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;
        private Mock<IOracle> _oracle;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();

            _subject = new TestCaseValidatorFactory(_oracle.Object);
        }

        [Test]
        [TestCase("encrypt", "aft", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "AFT", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypT", "CTR", typeof(TestCaseValidatorCounterEncrypt))]
        [TestCase("DECRYPT", "ctr", typeof(TestCaseValidatorCounterDecrypt))]

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
                        TestType = testType,
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
