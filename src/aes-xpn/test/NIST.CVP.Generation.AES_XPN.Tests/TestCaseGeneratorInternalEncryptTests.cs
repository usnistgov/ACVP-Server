﻿using System;
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

namespace NIST.CVP.Generation.AES_XPN.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorInternalEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorInternalEncrypt(GetRandomMock().Object, GetCipherFactoryMock().Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup() { IVGeneration = "external", SaltGen = "external" }, false);

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

            var subject = new TestCaseGeneratorInternalEncrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup(), true);

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

            var subject = new TestCaseGeneratorInternalEncrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldNotInvokeEncryptionOperationIfNotSample()
        {
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(new BitString(128)));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorInternalEncrypt(GetRandomMock().Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup() { IVGeneration = "external", SaltGen = "external" }, false);

            aes.Verify(v => v.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()),
                Times.Never,
                "ProcessPayload should not have been invoked"
            );
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperationIfSample()
        {
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(new BitString(128)));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(1));

            var subject = new TestCaseGeneratorInternalEncrypt(random.Object, cipherFactory.Object, GetEngineFactoryMock().Object);

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
            var fakeTag = new BitString(new byte[] { 2 });

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(fakeCipher, fakeTag));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorInternalEncrypt(random.Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup() { IVGeneration = "internal", SaltGen = "internal" }, false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty((result.TestCase).AAD.ToString(), "AAD");
            Assert.IsNull((result.TestCase).CipherText, "CipherText");
            Assert.IsNull((result.TestCase).IV, "IV");
            Assert.IsNull((result.TestCase).Salt, "Salt");
            Assert.IsNotNull((result.TestCase).Key, "Key");
            Assert.IsNull((result.TestCase).Tag, "Tag");
            Assert.IsTrue(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        [TestCase("internal", true)]
        [TestCase("external", false)]
        public void ShouldPopulateIvWhenExternal(string ivGen, bool isNull)
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            var fakeTag = new BitString(new byte[] { 2 });

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(fakeCipher, fakeTag));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorInternalEncrypt(random.Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup() { IVGeneration = ivGen, SaltGen = "internal" }, false);

            Assert.AreEqual(isNull, ((TestCase)result.TestCase).IV == null);
        }

        [Test]
        [TestCase("internal", true)]
        [TestCase("external", false)]
        public void ShouldPopulateSaltWhenExternal(string saltGen, bool isNull)
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            var fakeTag = new BitString(new byte[] { 2 });

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<AeadModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherAeadResult(fakeCipher, fakeTag));

            var cipherFactory = GetCipherFactoryMock();
            cipherFactory
                .Setup(s => s.GetAeadCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(aes.Object);

            var subject = new TestCaseGeneratorInternalEncrypt(random.Object, cipherFactory.Object, GetEngineFactoryMock().Object);

            var result = subject.Generate(new TestGroup() { IVGeneration = "internal", SaltGen = saltGen }, false);

            Assert.AreEqual(isNull, ((TestCase)result.TestCase).Salt == null);
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