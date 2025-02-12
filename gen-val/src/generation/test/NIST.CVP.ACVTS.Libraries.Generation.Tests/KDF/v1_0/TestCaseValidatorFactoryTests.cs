﻿using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.v1_0
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
        public void ShouldReturnCorrectValidatorType()
        {
            var testVectorSet = new TestVectorSet
            {
                TestGroups = new List<TestGroup>
                {
                    new TestGroup
                    {
                        Tests = new List<TestCase>
                        {
                            new TestCase()
                        }
                    }
                }
            };

            var result = _subject.GetValidators(testVectorSet);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.InstanceOf(typeof(TestCaseValidator)));
        }
    }
}
