using System;
using Moq;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorPartialBlockEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorPartialBlockEncrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult("Fail"));

            var subject = new TestCaseGeneratorPartialBlockEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGeneratorPartialBlockEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAESMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(0));

            var subject = new TestCaseGeneratorPartialBlockEncrypt(random.Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), true);

            aes.Verify(v => v.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()),
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
                .Setup(s => s.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult(fakeCipher));

            var subject = new TestCaseGeneratorPartialBlockEncrypt(random.Object, aes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).IV.ToString(), "IV");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void GeneratedCipherTextShouldDecryptBackToPlainText()
        {
            var aes_ctr = new AesCtr();
            var subject = new TestCaseGeneratorPartialBlockEncrypt(new Random800_90(), aes_ctr);
            var testGroup = GetTestGroup();

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, false);
                Assume.That(result.Success);
                testGroup.Tests.Add(result.TestCase);
            }

            foreach (TestCase testCase in testGroup.Tests)
            {
                var decryptResult = aes_ctr.DecryptBlock(testCase.Key, testCase.CipherText, testCase.IV);
                Assert.AreEqual(testCase.PlainText, decryptResult.Result);
            }
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IAesCtr> GetAESMock()
        {
            return new Mock<IAesCtr>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                KeyLength = 128,
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 128))
            };
        }
    }
}
