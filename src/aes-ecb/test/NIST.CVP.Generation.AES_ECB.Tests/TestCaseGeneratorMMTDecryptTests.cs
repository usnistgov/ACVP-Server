﻿using System;
using Moq;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMMTDecryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, GetAesMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherResult("Fail"));

            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception());

            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAesMock();

            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), true);

            aes.Verify(v => v.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()),
                Times.AtLeastOnce,
                "BlockEncrypt should have been invoked"
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
            var aes = GetAesMock();
            aes
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherResult(fakeCipher));

            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(random.Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }     

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IModeBlockCipher<SymmetricCipherResult>> GetAesMock()
        {
            return new Mock<IModeBlockCipher<SymmetricCipherResult>>();
        }
    }
}