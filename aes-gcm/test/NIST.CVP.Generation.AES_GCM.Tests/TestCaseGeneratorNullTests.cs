using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
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
