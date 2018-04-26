using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorCounterDecryptTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GenerateShouldReturnTestCaseGenerateResponse(bool isSample)
        {
            var subject = new TestCaseGeneratorCounterDecrypt(GetRandomMock().Object, GetTDESMock().Object);

            var result = subject.Generate(GetTestGroup(true, true), isSample);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var tdes = GetTDESMock();
            tdes
                .Setup(s => s.Decrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()))
                .Returns(new SymmetricCounterResult("Fail"));

            var subject = new TestCaseGeneratorCounterDecrypt(GetRandomMock().Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var tdes = GetTDESMock();
            tdes
                .Setup(s => s.Decrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()))
                .Throws(new Exception());

            var subject = new TestCaseGeneratorCounterDecrypt(GetRandomMock().Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperationWhenIsSampleIsTrue()
        {
            var tdes = GetTDESMock();

            var subject = new TestCaseGeneratorCounterDecrypt(GetRandomMock().Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            tdes.Verify(v => v.Decrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()),
                Times.AtLeastOnce,
                "Decrypt should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldNotInvokeEncryptionOperationWhenIsSampleIsFalse()
        {
            var tdes = GetTDESMock();

            var subject = new TestCaseGeneratorCounterDecrypt(GetRandomMock().Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(true, true), false);

            tdes.Verify(v => v.Decrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()),
                Times.Never,
                "Decrypt should not have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            var fakeIvs = new List<BitString>();

            var tdes = GetTDESMock();
            tdes
                .Setup(s => s.Decrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<ICounter>()))
                .Returns(new SymmetricCounterResult(fakeCipher, fakeIvs));

            var subject = new TestCaseGeneratorCounterDecrypt(GetRandomMock().Object, tdes.Object);

            var result = subject.Generate(GetTestGroup(true, true), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Ivs.ToString(), "Ivs");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsTrue(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void GeneratedPlainTextShouldEncryptBackToCipherText()
        {
            var tdes_ctr = new TdesCtr();
            var subject = new TestCaseGeneratorCounterDecrypt(new Random800_90(), tdes_ctr);
            var testGroup = GetTestGroup(true, false);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, true);
                Assume.That(result.Success);
                testGroup.Tests.Add(result.TestCase);
            }

            Assert.AreEqual(1, testGroup.Tests.Count);

            var testCase = (TestCase)testGroup.Tests.First();
            var encryptResult = tdes_ctr.Encrypt(testCase.Key, testCase.PlainText,
                new TestableCounter(Cipher.TDES, testCase.Ivs));
            Assert.AreEqual(testCase.CipherText, encryptResult.Result);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var mock = new Mock<IRandom800_90>();
            mock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            return mock;
        }

        private Mock<ITdesCtr> GetTDESMock()
        {
            return new Mock<ITdesCtr>();
        }

        private TestGroup GetTestGroup(bool increment, bool overflow)
        {
            return new TestGroup
            {
                NumberOfKeys = 3,
                IncrementalCounter = increment,
                OverflowCounter = overflow
            };
        }
    }
}
