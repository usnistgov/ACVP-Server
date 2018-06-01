using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using Cipher = NIST.CVP.Crypto.Common.Symmetric.CTR.Enums.Cipher;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorCounterEncryptTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GenerateShouldReturnTestCaseGenerateResponse(bool isSample)
        {
            var subject = new TestCaseGeneratorCounterEncrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = subject.Generate(GetTestGroup(true, true), isSample);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()))
                .Returns(new SymmetricCounterResult("Fail"));

            var subject = new TestCaseGeneratorCounterEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()))
                .Throws(new Exception());

            var subject = new TestCaseGeneratorCounterEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperationWhenIsSampleIsTrue()
        {
            var aes = GetAESMock();

            var subject = new TestCaseGeneratorCounterEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            aes.Verify(v => v.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()),
                Times.AtLeastOnce,
                "Encrypt should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldNotInvokeEncryptionOperationWhenIsSampleIsFalse()
        {
            var aes = GetAESMock();

            var subject = new TestCaseGeneratorCounterEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(GetTestGroup(true, true), false);

            aes.Verify(v => v.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()),
                Times.Never,
                "Encrypt should not have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            var fakeIvs = new List<BitString>();

            var aes = GetAESMock();
            aes
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()))
                .Returns(new SymmetricCounterResult(fakeCipher, fakeIvs));

            var subject = new TestCaseGeneratorCounterEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsTrue(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void GeneratedCipherTextShouldDecryptBackToPlainText()
        {
            var aes_ctr = new AesCtr();
            var subject = new TestCaseGeneratorCounterEncrypt(new Random800_90(), aes_ctr);
            var testGroup = GetTestGroup(true, false);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, true);
                Assume.That(result.Success);
                testGroup.Tests.Add(result.TestCase);
            }

            Assert.AreEqual(1, testGroup.Tests.Count);

            var testCase = (TestCase) testGroup.Tests.First();
            var solveIVs = aes_ctr.CounterEncrypt(testCase.Key, testCase.PlainText, testCase.CipherText);
            var decryptResult = aes_ctr.Decrypt(testCase.Key, testCase.CipherText, new TestableCounter(Cipher.AES, solveIVs.IVs));
            Assert.AreEqual(testCase.PlainText, decryptResult.Result);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var mock = new Mock<IRandom800_90>();
            mock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            return mock;
        }

        private Mock<IAesCtr> GetAESMock()
        {
            return new Mock<IAesCtr>();
        }

        private TestGroup GetTestGroup(bool increment, bool overflow)
        {
            return new TestGroup
            {
                KeyLength = 128,
                IncrementalCounter = increment,
                OverflowCounter = overflow
            };
        }
    }
}
