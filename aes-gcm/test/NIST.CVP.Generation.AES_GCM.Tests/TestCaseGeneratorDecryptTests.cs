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
    public class TestCaseGeneratorDecryptTests
    {
        [Test]
        public void ShouldReturnInternal()
        {
            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, GetAESMock().Object);

            Assert.AreEqual("internal", sut.IVGen);
        }

        [Test]
        public void ShouldReturnDecrypt()
        {
            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, GetAESMock().Object);

            Assert.AreEqual("decrypt", sut.Direction);
        }

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = sut.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new EncryptionResult("Fail"));

            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, aes.Object);

            var result = sut.Generate(new TestGroup(), false);

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

            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, aes.Object);

            var result = sut.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAESMock();

            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, aes.Object);

            var result = sut.Generate(new TestGroup(), true);

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
            random.Setup(s => s.GetDifferentBitStringOfSameSize(fakeTag))
                .Returns(new BitString(new byte[] { 4 }));
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new EncryptionResult(fakeCipher, fakeTag));

            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            var result = sut.Generate(new TestGroup(), false);

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

        [Test]
        public void GenerateShouldSometimesMangleTag()
        {
            if (!TestCaseGeneratorDecrypt._SHOULD_CREATE_FAILURE_TESTS)
            {
                Assume.That(1 == 0, "Ignore test while decryption 'expected failure' tests turned off.");
                return;
            }

            var fakeCipher = new BitString(new byte[] { 1 });
            var fakeTag = new BitString(new byte[] { 2 });
            var mangledTag = new BitString(new byte[] { 42 });
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            random.Setup(s => s.GetDifferentBitStringOfSameSize(fakeTag))
                .Returns(mangledTag);
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new EncryptionResult(fakeCipher, fakeTag));

            TestCaseGeneratorDecrypt sut =
                new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            bool originalFakeTagHit = false;
            bool mangledTagHit = false;
            for (int i = 0; i < 4; i++)
            {
                random
                    .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(i);

                var result = sut.Generate(new TestGroup(), false);
                var tc = (TestCase)result.TestCase;
                if (tc.Tag == fakeTag)
                {
                    originalFakeTagHit = true;
                    Assert.IsFalse(tc.FailureTest, "Should not be a failure test");
                }
                if (tc.Tag == mangledTag)
                {
                    mangledTagHit = true;
                    Assert.IsTrue(tc.FailureTest, "Should be a failure test");
                }
            }

            Assert.IsTrue(originalFakeTagHit, nameof(originalFakeTagHit));
            Assert.IsTrue(mangledTagHit, nameof(mangledTagHit));
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
