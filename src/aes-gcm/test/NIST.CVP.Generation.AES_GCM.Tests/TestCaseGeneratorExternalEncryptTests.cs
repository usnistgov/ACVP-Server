using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorExternalEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorExternalEncrypt subject =
                new TestCaseGeneratorExternalEncrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new SymmetricCipherAeadResult("Fail"));

            TestCaseGeneratorExternalEncrypt subject =
                new TestCaseGeneratorExternalEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Throws(new Exception());

            TestCaseGeneratorExternalEncrypt subject =
                new TestCaseGeneratorExternalEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAESMock();

            TestCaseGeneratorExternalEncrypt subject =
                new TestCaseGeneratorExternalEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), true);

            aes.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()),
                Times.AtLeastOnce,
                "BlockEncrypt should have been invoked"
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
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new SymmetricCipherAeadResult(fakeCipher, fakeTag));

            TestCaseGeneratorExternalEncrypt subject =
                new TestCaseGeneratorExternalEncrypt(random.Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).AAD.ToString(), "AAD");
            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).IV.ToString(), "IV");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Tag.ToString(), "Tag");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IAES_GCM> GetAESMock()
        {
            return new Mock<IAES_GCM>();
        }
    }
}
