using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.AES_FF.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase(BlockCipherDirections.Encrypt, 128, typeof(TestCaseGenerator))]
        [TestCase(BlockCipherDirections.Decrypt, 128, typeof(TestCaseGenerator))]

        [TestCase(BlockCipherDirections.Encrypt, 196, typeof(TestCaseGenerator))]
        [TestCase(BlockCipherDirections.Decrypt, 196, typeof(TestCaseGenerator))]

        [TestCase(BlockCipherDirections.Encrypt, 256, typeof(TestCaseGenerator))]
        [TestCase(BlockCipherDirections.Decrypt, 256, typeof(TestCaseGenerator))]
        public void ShouldReturnProperGenerator(BlockCipherDirections direction, int keySize, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                KeyLength = keySize
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(new TestGroup()
            {
                Function = BlockCipherDirections.Encrypt
            });
            Assert.IsNotNull(generator);
        }
    }
}
