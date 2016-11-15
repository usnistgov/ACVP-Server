using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "external", "ExternalEncrypt")]
        [TestCase("Encrypt", "external", "ExternalEncrypt")]
        [TestCase("Encrypt", "EXternaL", "ExternalEncrypt")]
        [TestCase("ENcrypt", "External", "ExternalEncrypt")]
        [TestCase("Encrypt", "Internal", "InternalEncrypt")]
        [TestCase("encrypt", "internal", "InternalEncrypt")]
        [TestCase("Decrypt", "Internal", "Decrypt")]
        [TestCase("decrypt", "internal", "Decrypt")]
        [TestCase("decrypt", "external", "Decrypt")]
        [TestCase("decrypt", "junk", "Decrypt")]
        [TestCase("Junk", "internal", "Null")]
        [TestCase("encrypt", "junk", "Null")]
        public void ShouldReturnProperGenerator(string direction, string ivGen, string genNameHint)
        {
            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(direction, ivGen);
            Assume.That(generator != null);
            Assert.IsTrue(generator.GetType().Name.EndsWith(genNameHint));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator("", "");
            Assert.IsNotNull(generator);
        }
    }
}
