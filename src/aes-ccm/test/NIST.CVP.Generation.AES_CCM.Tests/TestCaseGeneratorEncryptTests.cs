using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
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
    public class TestCaseGeneratorEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorEncrypt(GetRandomMock().Object, GetCipherFactoryMock().Object, GetEngineFactoryMock().Object);
    
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

            var subject = new TestCaseGeneratorEncrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

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

            var subject = new TestCaseGeneratorEncrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object); ;

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

            var subject = new TestCaseGeneratorEncrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

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

            var subject = new TestCaseGeneratorEncrypt(random.Object, cipherFactory.Object, GetEngineFactoryMock().Object);

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
        [TestCase("No change 0 bits", 0, 10)]
        [TestCase("No change 10 bits", 10, 10)]
        [TestCase("No change 32 bits", 32, 10)]
        [TestCase("no change 33 bits", 33, 10)]
        [TestCase("no change 256 bits", 256, 10)]
        [TestCase("change 257 bits", 257, 1)]
        [TestCase("change 65536", 65536, 1)]
        public void ShouldChangeNumberOfTestCasesWhenAadLenGt32bytes(string testLabel, int aadLen, int expectedNumberOfCases)
        {
            var subject = new TestCaseGeneratorEncrypt(GetRandomMock().Object, GetCipherFactoryMock().Object, GetEngineFactoryMock().Object);

            var tg = new TestGroup
            {
                AADLength = aadLen
            };

            var result = subject.Generate(tg, true);

            Assert.AreEqual(expectedNumberOfCases, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReuseKeyForReuseKeyGroup(bool shouldReuse)
        {
            var tg = new TestGroup
            {
                GroupReusesKeyForTestCases = shouldReuse,
                AADLength = 32,
                Function = "encrypt",
                IVLength = 13*8,
                KeyLength = 128,
                PTLength = 32,
                TagLength = 16*8
            };

            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(new BitString(128)));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorEncrypt(new Random800_90(), cipherFactory.Object, GetEngineFactoryMock().Object);
            var test1 = subject.Generate(tg, true);
            var test2 = subject.Generate(tg, true);

            if (shouldReuse)
            {
                Assert.AreEqual((test1.TestCase).Key, (test2.TestCase).Key);
            }
            else
            {
                Assert.AreNotEqual((test1.TestCase).Key, (test2.TestCase).Key);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReuseNonceForReuseNonceGroup(bool shouldReuse)
        {
            var tg = new TestGroup
            {
                GroupReusesNonceForTestCases = shouldReuse,
                AADLength = 32,
                Function = "encrypt",
                IVLength = 13 * 8,
                KeyLength = 128,
                PTLength = 32,
                TagLength = 16 * 8
            };

            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(new BitString(128)));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorEncrypt(new Random800_90(), cipherFactory.Object, GetEngineFactoryMock().Object);
            var test1 = subject.Generate(tg, true);
            var test2 = subject.Generate(tg, true);

            if (shouldReuse)
            {
                Assert.AreEqual((test1.TestCase).IV, (test2.TestCase).IV);
            }
            else
            {
                Assert.AreNotEqual((test1.TestCase).IV, (test2.TestCase).IV);
            }
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
