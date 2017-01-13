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
        [TestCase("encrypt", "MMT",  "Encrypt")]
        [TestCase("Encrypt", "MmT", "Encrypt")]
        [TestCase("ENcrypt", "MMT", "Encrypt")]
        [TestCase("Decrypt", "mMT", "Decrypt")]
        [TestCase("decrypt", "MMt", "Decrypt")]
        [TestCase("Junk", "", "Null")]
        [TestCase("", "", "Null")]
        public void ShouldReturnProperGenerator(string direction, string testType, string genNameHint)
        {
            var subject = new TestCaseGeneratorFactory(null, null, null);
            var generator = subject.GetCaseGenerator(direction, testType);
            Assume.That(generator != null);
            Assert.IsTrue(generator.GetType().Name.EndsWith(genNameHint));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null, null, null);
            var generator = subject.GetCaseGenerator("", "");
            Assert.IsNotNull(generator);
        }
    }
}
