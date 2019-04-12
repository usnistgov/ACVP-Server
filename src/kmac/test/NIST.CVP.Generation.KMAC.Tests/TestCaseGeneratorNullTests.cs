using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.KMAC.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorNullTests
    {
        [Test]
        public async Task ShouldReturnErrorResponseForIsSampleCall()
        {
            var subject = new TestCaseGeneratorNull();
            var result = await subject.GenerateAsync(new TestGroup(), false);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("This is the null generator -- nothing is generated", result.ErrorMessage);
        }

        [Test]
        public void ShouldHave0NumberOfCases()
        {
            var subject = new TestCaseGeneratorNull();
            Assert.AreEqual(0, subject.NumberOfTestCasesToGenerate);
        }
    }
}
