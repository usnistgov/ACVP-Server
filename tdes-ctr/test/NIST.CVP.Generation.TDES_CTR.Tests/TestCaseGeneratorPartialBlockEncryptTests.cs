using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorPartialBlockEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorPartialBlockEncrypt(GetRandomMock().Object, GetTDESMock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var tdes = GetTDESMock();
            tdes
                .Setup(s => s.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new EncryptionResult("Fail"));

            var subject = new TestCaseGeneratorPartialBlockEncrypt(GetRandomMock().Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var tdes = GetTDESMock();
            tdes
                .Setup(s => s.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGeneratorPartialBlockEncrypt(GetRandomMock().Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var tdes = GetTDESMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(0));

            var subject = new TestCaseGeneratorPartialBlockEncrypt(random.Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(), true);

            tdes.Verify(v => v.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()),
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
            var tdes = GetTDESMock();
            tdes
                .Setup(s => s.EncryptBlock(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new EncryptionResult(fakeCipher));

            var subject = new TestCaseGeneratorPartialBlockEncrypt(random.Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Iv.ToString(), "Iv");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void GeneratedCipherTextShouldDecryptBackToPlainText()
        {
            var tdes_ctr = new TdesCtr();
            var subject = new TestCaseGeneratorPartialBlockEncrypt(new Random800_90(), tdes_ctr);
            var testGroup = GetTestGroup();

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, false);
                Assume.That(result.Success);
                testGroup.Tests.Add(result.TestCase);
            }

            foreach (TestCase testCase in testGroup.Tests)
            {
                var decryptResult = tdes_ctr.DecryptBlock(testCase.Key, testCase.CipherText, testCase.Iv);
                Assert.AreEqual(testCase.PlainText, decryptResult.PlainText);
            }
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            return random;
        }

        private Mock<ITdesCtr> GetTDESMock()
        {
            return new Mock<ITdesCtr>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                NumberOfKeys = 3,
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 64))
            };
        }
    }
}
