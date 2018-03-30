using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
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
        public void ShouldReturnErrorForRedoGenerate()
        {
            var subject = new TestCaseGeneratorNull();
            var result = subject.Generate(new TestGroup(), new TestCase());
            Assert.IsFalse(result.Success);
        }
    }
}
