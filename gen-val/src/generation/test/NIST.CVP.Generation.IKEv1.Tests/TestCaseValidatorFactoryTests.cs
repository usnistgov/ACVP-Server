﻿using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.KDF_Components.v1_0.IKEv1;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv1.Tests
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

            Assert.AreEqual(1, result.Count());
            Assert.IsInstanceOf(typeof(TestCaseValidator), result.First());
        }
    }
}