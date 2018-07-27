using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XTS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("ENcrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("dECrypt", typeof(TestCaseGeneratorDecrypt))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup
            {
                Direction = direction,
            };

            var engineFactory = new Mock<IBlockCipherEngineFactory>();
            var cipherFactory = new Mock<IModeBlockCipherFactory>();

            var subject = new TestCaseGeneratorFactory(null, engineFactory.Object, cipherFactory.Object);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            TestGroup testGroup = new TestGroup
            {
                Direction = string.Empty,
                TestType = string.Empty
            };

            var engineFactory = new Mock<IBlockCipherEngineFactory>();
            var cipherFactory = new Mock<IModeBlockCipherFactory>();

            var subject = new TestCaseGeneratorFactory(null, engineFactory.Object, cipherFactory.Object);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
