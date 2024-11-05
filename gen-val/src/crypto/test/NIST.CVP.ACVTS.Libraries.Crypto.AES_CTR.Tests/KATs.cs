using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class KATs
    {
        private static int[] _keyLens = { 128, 192, 256 };

        private Mock<ICounter> _mockCounter;
        private CtrBlockCipher _subject;

        [SetUp]
        public void Setup()
        {
            _mockCounter = new Mock<ICounter>();
            _subject = new CtrBlockCipher(new AesEngine(), _mockCounter.Object);
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldGfSBoxCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetGfSBox(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);

                Assert.That(result.Success, Is.True, nameof(result.Success));
                Assert.That(result.Result, Is.EqualTo(test.CipherText), test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldKeySBoxCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetKeySBox(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);

                Assert.That(result.Success, Is.True, nameof(result.Success));
                Assert.That(result.Result, Is.EqualTo(test.CipherText), test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldVarKeyCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetVarKey(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);

                Assert.That(result.Success, Is.True, nameof(result.Success));
                Assert.That(result.Result, Is.EqualTo(test.CipherText), test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldVarTxtCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetVarTxt(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);

                Assert.That(result.Success, Is.True, nameof(result.Success));
                Assert.That(result.Result, Is.EqualTo(test.CipherText), test.CipherText.ToHex());
            }
        }
    }
}
