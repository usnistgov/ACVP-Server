﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBCI.v1_0;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CBCI
{
    [TestFixture]
    public class TestCaseGeneratorNullTests
    {
        [Test]
        public async Task ShouldReturnErrorResponseForIsSampleCall()
        {
            var subject = new TestCaseGeneratorNull();
            var result = await subject.GenerateAsync(new TestGroup(), false);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("This is the null generator -- nothing is generated"));
        }

        [Test]
        public void ShouldHave0NumberOfCases()
        {
            var subject = new TestCaseGeneratorNull();
            Assert.That(subject.NumberOfTestCasesToGenerate, Is.EqualTo(0));
        }
    }
}
