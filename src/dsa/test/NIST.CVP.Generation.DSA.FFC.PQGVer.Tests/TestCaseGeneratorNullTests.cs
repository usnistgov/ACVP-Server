using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorNullTests
    {
        [Test]
        public void ShouldReturnErrorForInitialGenerate()
        {
            var subject = new TestCaseGeneratorNull();
            var result = subject.Generate(new TestGroup(), false);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorForCaseGenerate()
        {
            var subject = new TestCaseGeneratorNull();
            var result = subject.Generate(new TestGroup(), new TestCase());
            Assert.IsFalse(result.Success);
        }
    }
}
