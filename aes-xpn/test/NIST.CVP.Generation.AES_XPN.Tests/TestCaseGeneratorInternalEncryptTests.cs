using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XPN.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorInternalEncryptTests
    {
        private TestCaseGeneratorInternalEncrypt _subject;

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var aes = GetAESMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(1));

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = _subject.Generate(new TestGroup() { IVGeneration = "external", SaltGen = "external" }, false);

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
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(1));

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);


            var result = _subject.Generate(new TestGroup(), true);

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
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(1));

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = _subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldNotInvokeEncryptionOperationIfNotSample()
        {
            var aes = GetAESMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(1));

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = _subject.Generate(new TestGroup() { IVGeneration = "external", SaltGen = "external" }, false);

            aes.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()), 
                Times.Never, 
                "BlockEncrypt should not have been invoked"
            );
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperationIfSample()
        {
            var aes = GetAESMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(1));

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = _subject.Generate(new TestGroup(), true);

            aes.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()),
                Times.AtLeastOnce,
                "BlockEncrypt should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var aes = GetAESMock();
            aes.
                Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(
                    new SymmetricCipherAeadResult(
                        new BitString(new byte[] { 1 }), 
                        new BitString(new byte[] { 2 })
                    )
                );

            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = _subject.Generate(new TestGroup() { IVGeneration = "internal", SaltGen = "internal" }, false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).AAD.ToString(), "AAD");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsNull(((TestCase)result.TestCase).CipherText, "CipherText");
            Assert.IsNull(((TestCase)result.TestCase).IV, "IV");
            Assert.IsNull(((TestCase)result.TestCase).Salt, "Salt");
            Assert.IsNotNull(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNull(((TestCase)result.TestCase).Tag, "Tag");
            Assert.IsTrue(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        [TestCase("internal", true)]
        [TestCase("external", false)]
        public void ShouldPopulateIvWhenExternal(string ivGen, bool isNull)
        {
            var aes = GetAESMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = _subject.Generate(new TestGroup() { IVGeneration = ivGen, SaltGen = "internal" }, false);

            Assert.AreEqual(isNull, ((TestCase)result.TestCase).IV == null);
        }

        [Test]
        [TestCase("internal", true)]
        [TestCase("external", false)]
        public void ShouldPopulateSaltWhenExternal(string saltGen, bool isNull)
        {
            var aes = GetAESMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            _subject =
                new TestCaseGeneratorInternalEncrypt(random.Object, aes.Object);

            var result = _subject.Generate(new TestGroup() { IVGeneration = "internal", SaltGen = saltGen }, false);

            Assert.AreEqual(isNull, ((TestCase)result.TestCase).Salt == null);
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
