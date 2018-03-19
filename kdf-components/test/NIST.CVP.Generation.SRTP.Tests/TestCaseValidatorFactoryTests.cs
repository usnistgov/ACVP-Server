using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.Tests
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
