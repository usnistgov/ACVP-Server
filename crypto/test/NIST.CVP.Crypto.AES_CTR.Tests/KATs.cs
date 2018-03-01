using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class KATs
    {
        private readonly IAesCtr _subject = new AesCtr();
        private static int[] _keyLens = {128, 192, 256};

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldGfSBoxCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetGfSBox(keyLen))
            {
                var result = _subject.EncryptBlock(test.Key, test.PlainText, test.IV);

                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldKeySBoxCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetKeySBox(keyLen))
            {
                var result = _subject.EncryptBlock(test.Key, test.PlainText, test.IV);

                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldVarKeyCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetVarKey(keyLen))
            {
                var result = _subject.EncryptBlock(test.Key, test.PlainText, test.IV);

                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldVarTxtCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetVarTxt(keyLen))
            {
                var result = _subject.EncryptBlock(test.Key, test.PlainText, test.IV);

                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }
    }
}
