using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorDecryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorDecrypt(GetRandomMock().Object, GetCipherFactoryMock().Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult("Fail"));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);
            
            var subject = new TestCaseGeneratorDecrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Throws(new Exception());

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorDecrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(new BitString(128)));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorDecrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup(), true);

            aes.Verify(v => v.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()),
                Times.AtLeastOnce,
                "ProcessPayload should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeCipher = new BitString(new byte[] { 1 });

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            random.Setup(s => s.GetDifferentBitStringOfSameSize(fakeCipher))
                .Returns(new BitString(new byte[] { 4 }));

            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(fakeCipher));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorDecrypt(random.Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty((result.TestCase).AAD.ToString(), "AAD");
            Assert.IsNotEmpty((result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty((result.TestCase).IV.ToString(), "IV");
            Assert.IsNotEmpty((result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty((result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void GenerateShouldSometimesMangleCipherText()
        {
            var fakeCipher = new BitString(new byte[] { 1 });

            var mangledCipher = new BitString(new byte[] { 2 });

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            random.Setup(s => s.GetDifferentBitStringOfSameSize(fakeCipher))
                .Returns(mangledCipher);

            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(fakeCipher));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorDecrypt(random.Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var originalFakeCipherHit = false;
            var mangledCipherHit = false;
            for (var i = 0; i < 4; i++)
            {
                random
                    .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(i);

                var result = subject.Generate(new TestGroup(), false);
                var tc = result.TestCase;
                if (tc.CipherText == fakeCipher)
                {
                    originalFakeCipherHit = true;
                    Assert.IsTrue(tc.TestPassed, "Should not be a failure test");
                }

                if (tc.CipherText == mangledCipher)
                {
                    mangledCipherHit = true;
                    Assert.IsFalse(tc.TestPassed, "Should be a failure test");
                }
            }

            Assert.IsTrue(originalFakeCipherHit, nameof(originalFakeCipherHit));
            Assert.IsTrue(mangledCipherHit, nameof(mangledCipherHit));
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IAeadModeBlockCipherFactory> GetCipherFactoryMock()
        {
            return new Mock<IAeadModeBlockCipherFactory>();
        }

        private Mock<IBlockCipherEngineFactory> GetEngineFactoryMock()
        {
            return new Mock<IBlockCipherEngineFactory>();
        }

        private Mock<IAeadModeBlockCipher> GetAesMock()
        {
            return new Mock<IAeadModeBlockCipher>();
        }
    }
}
