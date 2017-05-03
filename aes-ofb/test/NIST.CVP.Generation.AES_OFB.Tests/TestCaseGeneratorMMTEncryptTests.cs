using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_OFB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorMMTEncrypt subject =
                new TestCaseGeneratorMMTEncrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new EncryptionResult("Fail"));

            TestCaseGeneratorMMTEncrypt subject =
                new TestCaseGeneratorMMTEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            TestCaseGeneratorMMTEncrypt subject =
                new TestCaseGeneratorMMTEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAESMock();

            TestCaseGeneratorMMTEncrypt subject =
                new TestCaseGeneratorMMTEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), true);

            aes.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()),
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
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new EncryptionResult(fakeCipher));

            TestCaseGeneratorMMTEncrypt subject =
                new TestCaseGeneratorMMTEncrypt(random.Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void GeneratedCipherTextShouldDecryptBackToPlainText()
        {
            var ri = new RijndaelInternals();
            var rf = new RijndaelFactory(ri);
            var aes_ofb = new Crypto.AES_OFB.AES_OFB(rf);
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), aes_ofb);
            var testGroup = new TestGroup { KeyLength = 128 };

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, false);
                Assume.That(result.Success);
                testGroup.Tests.Add(result.TestCase);
            }

            foreach (TestCase testCase in testGroup.Tests)
            {
                var decryptResult = aes_ofb.BlockEncrypt(testCase.IV, testCase.Key, testCase.PlainText);
                Assert.AreEqual(testCase.CipherText, decryptResult.CipherText);
            }
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IAES_OFB> GetAESMock()
        {
            return new Mock<IAES_OFB>();
        }
    }
}
