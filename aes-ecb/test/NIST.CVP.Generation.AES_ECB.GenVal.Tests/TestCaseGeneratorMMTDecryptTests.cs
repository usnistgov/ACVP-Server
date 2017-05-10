using System;
using Moq;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.GenVal.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTDecryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new EncryptionResult("Fail"));

            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
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
            var aes = GetAESMock();

            TestCaseGeneratorMMTDecrypt subject =
                new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), true);

            aes.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false),
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
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new EncryptionResult(fakeCipher));

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

        private Mock<IAES_ECB> GetAESMock()
        {
            return new Mock<IAES_ECB>();
        }
    }
}
