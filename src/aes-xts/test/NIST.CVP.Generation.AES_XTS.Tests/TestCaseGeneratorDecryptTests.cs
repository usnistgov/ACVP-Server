using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.AES_XTS;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XTS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorDecryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(0));
            random
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(10);

            var aes = GetAESMock();
            aes
                .Setup(s => s.Decrypt(It.IsAny<XtsKey>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult(new BitString("ABCD")));

            aes
                .Setup(s => s.GetIFromInteger(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            var subject = new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedDecryption()
        {
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(0));
            random
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(10);

            var aes = GetAESMock();
            aes
                .Setup(s => s.Decrypt(It.IsAny<XtsKey>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult("Fail"));

            var subject = new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionDecryption()
        {
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(0));
            random
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(10);

            var aes = GetAESMock();
            aes
                .Setup(s => s.Decrypt(It.IsAny<XtsKey>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeDecryptionOperation()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.Decrypt(It.IsAny<XtsKey>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult(new BitString("ABCD")));

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(0));
            random
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(10);

            var subject = new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), true);

            aes.Verify(v => v.Decrypt(It.IsAny<XtsKey>(), It.IsAny<BitString>(), It.IsAny<BitString>()),
                Times.AtLeastOnce,
                "BlockDecrypt should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            random
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(10);

            var aes = GetAESMock();
            aes
                .Setup(s => s.Decrypt(It.IsAny<XtsKey>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult(new BitString("ABCD")));

            aes
                .Setup(s => s.GetIFromInteger(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            var subject = new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).I.ToString(), "I");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SequenceNumber.ToString(), "Sequence Number");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IAesXts> GetAESMock()
        {
            return new Mock<IAesXts>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Direction = "Decrypt",
                KeyLen = 128,
                PtLen = 128,
                TweakMode = "number"
            };
        }
    }
}
