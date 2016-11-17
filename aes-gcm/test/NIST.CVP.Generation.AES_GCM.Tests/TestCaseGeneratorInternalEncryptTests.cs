using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class TestCaseGeneratorInternalEncryptTests
    {

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorInternalEncrypt sut =
                new TestCaseGeneratorInternalEncrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = sut.Generate(new TestGroup());

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldNotInvokeEncryptionOperation()
        {
            var aes = GetAESMock();

            TestCaseGeneratorInternalEncrypt sut =
                new TestCaseGeneratorInternalEncrypt(GetRandomMock().Object, aes.Object);

            var result = sut.Generate(new TestGroup());

            aes.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()), 
                Times.Never, 
                "BlockEncrypt should not have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var aes = GetAESMock();
            aes.
                Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(
                    new EncryptionResult(
                        new BitString(new byte[] { 1 }), 
                        new BitString(new byte[] { 2 })
                    )
                );

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            

            TestCaseGeneratorInternalEncrypt sut =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = sut.Generate(new TestGroup());

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).AAD.ToString(), "AAD");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsNull(((TestCase)result.TestCase).CipherText, "CipherText");
            Assert.IsNull(((TestCase)result.TestCase).IV, "IV");
            Assert.IsNotNull(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNull(((TestCase)result.TestCase).Tag, "Tag");
            Assert.IsTrue(result.TestCase.Deferred, "Deferred");
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
