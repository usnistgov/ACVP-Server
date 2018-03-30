using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorNullTests
    {
        [Test]
        public void ShouldReturnErrorForAnyTestCase()
        {
            var testCase = new TestCase();
            var subject = new TestCaseValidatorNull(testCase);
            var result = subject.Validate(new TestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.AreEqual("Test type was not found", result.Reason);
        }
    }
}
