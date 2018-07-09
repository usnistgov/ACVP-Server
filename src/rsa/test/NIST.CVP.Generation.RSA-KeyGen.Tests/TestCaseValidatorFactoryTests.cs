using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new TestCaseValidatorFactory(null);
        }

        [Test]
        [TestCase("aft",   typeof(TestCaseValidatorAft))]
        [TestCase("aft",  typeof(TestCaseValidatorAft))]
        [TestCase("gdt",   typeof(TestCaseValidatorAft))]
        [TestCase("gdt",  typeof(TestCaseValidatorAft))]
        [TestCase("kat",   typeof(TestCaseValidatorKat))]
        [TestCase("kat",  typeof(TestCaseValidatorKat))]
        [TestCase("junk",  typeof(TestCaseValidatorNull))]
        [TestCase("junk", typeof(TestCaseValidatorNull))]
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
                TestGroups = new List<TestGroup>
                {
                    new TestGroup
                    {
                        TestType = testType,
                        Tests = new List<TestCase>
                        {
                            new TestCase
                            {
                                TestCaseId = 1,
                            }
                        }
                    }
                }
            };
        }
    }
}
