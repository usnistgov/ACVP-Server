using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "Encrypt")]
        [TestCase("Encrypt", "Encrypt")]
        [TestCase("ENcrypt", "Encrypt")]
        [TestCase("Decrypt", "Decrypt")]
        [TestCase("decrypt", "Decrypt")]
        [TestCase("Junk", "Null")]
        [TestCase("",  "Null")]
        public void ShouldReturnProperGenerator(string direction, string genNameHint)
        {
            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(direction);
            Assume.That(generator != null);
            Assert.IsTrue(generator.GetType().Name.EndsWith(genNameHint));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator("");
            Assert.IsNotNull(generator);
        }
    }
}
