using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests
{
    [TestFixture]
    public class TestCaseGeneratorDecryptTests
    {
        private Mock<IKeyWrapFactory> _iKeyWrapFactory;
        private Mock<IKeyWrap> _iKeyWrap;
        private Mock<IRandom800_90> _iRandom800_90;
        private TestCaseGeneratorDecrypt _subject;

        [SetUp]
        public void Setup()
        {
            _iKeyWrap = new Mock<IKeyWrap>();

            _iKeyWrapFactory = new Mock<IKeyWrapFactory>();
            _iKeyWrapFactory
                .Setup(s => s.GetKeyWrapInstance(It.IsAny<KeyWrapType>()))
                .Returns(_iKeyWrap.Object);

            _iRandom800_90 = new Mock<IRandom800_90>();
            _subject = new TestCaseGeneratorDecrypt(_iKeyWrapFactory.Object, _iRandom800_90.Object);
        }

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            _iKeyWrap
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new KeyWrapResult("Fail"));

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            _iKeyWrap
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Throws(new Exception());

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var result = _subject.Generate(GetTestGroup(), true);

            _iKeyWrap.Verify(v => v.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false),
                Times.AtLeastOnce,
                "Encrypt should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            _iRandom800_90
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            _iRandom800_90
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(4);
            _iKeyWrap
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new KeyWrapResult(fakeCipher));

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        public void ShouldSometimesMangleTestCase()
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            var mangledCipher = new BitString(new byte[] { 2 });
            _iRandom800_90
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            _iRandom800_90.Setup(s => s.GetDifferentBitStringOfSameSize(fakeCipher))
                .Returns(mangledCipher);
            _iKeyWrap
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new KeyWrapResult(fakeCipher));

            bool originalFakeCipherHit = false;
            bool mangledCipherHit = false;
            for (int i = 0; i < 4; i++)
            {
                _iRandom800_90
                    .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(i);

                var result = _subject.Generate(GetTestGroup(), false);
                var tc = (TestCase)result.TestCase;
                if (tc.CipherText == fakeCipher)
                {
                    originalFakeCipherHit = true;
                    Assert.IsFalse(tc.FailureTest, "Should not be a failure test");
                }
                if (tc.CipherText == mangledCipher)
                {
                    mangledCipherHit = true;
                    Assert.IsTrue(tc.FailureTest, "Should be a failure test");
                }
            }

            Assert.IsTrue(originalFakeCipherHit, nameof(originalFakeCipherHit));
            Assert.IsTrue(mangledCipherHit, nameof(mangledCipherHit));
        }

        private TestGroup GetTestGroup()
        {
            TestGroup tg = new TestGroup();
            tg.KwCipher = "";

            return tg;
        }
    }
}
