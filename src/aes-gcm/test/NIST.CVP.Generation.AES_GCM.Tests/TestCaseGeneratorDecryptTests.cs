using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorDecryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorDecrypt subject =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new SymmetricCipherAeadResult("Fail"));

            TestCaseGeneratorDecrypt subject =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, aes.Object);

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

            TestCaseGeneratorDecrypt subject =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAESMock();

            TestCaseGeneratorDecrypt subject =
                new TestCaseGeneratorDecrypt(GetRandomMock().Object, aes.Object);

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
            random.Setup(s => s.GetDifferentBitStringOfSameSize(fakeTag))
                .Returns(new BitString(new byte[] { 4 }));
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new SymmetricCipherAeadResult(fakeCipher, fakeTag));

            TestCaseGeneratorDecrypt subject =
                new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty((result.TestCase).AAD.ToString(), "AAD");
            Assert.IsNotEmpty((result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty((result.TestCase).IV.ToString(), "IV");
            Assert.IsNotEmpty((result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty((result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsNotEmpty((result.TestCase).Tag.ToString(), "Tag");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void GenerateShouldSometimesMangleTag()
        {
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
                .Returns(new SymmetricCipherAeadResult(fakeCipher, fakeTag));

            TestCaseGeneratorDecrypt subject =
                new TestCaseGeneratorDecrypt(random.Object, aes.Object);

            bool originalFakeTagHit = false;
            bool mangledTagHit = false;
            for (int i = 0; i < 4; i++)
            {
                random
                    .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(i);

                var result = subject.Generate(new TestGroup(), false);
                var tc = result.TestCase;
                if (tc.Tag == fakeTag)
                {
                    originalFakeTagHit = true;
                    Assert.IsTrue(tc.TestPassed, "Should not be a failure test");
                }
                if (tc.Tag == mangledTag)
                {
                    mangledTagHit = true;
                    Assert.IsFalse(tc.TestPassed, "Should be a failure test");
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
