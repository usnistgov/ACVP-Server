using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_CTR.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorNullTests
    {
        [Test]
        public async Task ShouldReturnErrorForAnyTestCase()
        {
            var testCase = new TestCase();
            var subject = new TestCaseValidatorNull(testCase);
            var result = await subject.ValidateAsync(new TestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.AreEqual("Test type was not found", result.Reason);
        }
    }
}
