using System.Collections;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CFB8.Tests
{
    [TestFixture, FastCryptoTest]
    public class KATs
    {
        private readonly AES_CFB8 _subject = new AES_CFB8(
                    new RijndaelFactory(
                        new RijndaelInternals()
                    )
                );

        #region TestData
        static IEnumerable _GetGFSBox128BitKey()
        {
            foreach (var item in KATDataCFB8.GetGFSBox128BitKey())
            {
                yield return new TestCaseData(item.IV.ToHex(), item);
            }
        }
        static IEnumerable _GetGFSBox192BitKey()
        {
            foreach (var item in KATDataCFB8.GetGFSBox192BitKey())
            {
                yield return new TestCaseData(item.IV.ToHex(), item);
            }
        }
        static IEnumerable _GetGFSBox256BitKey()
        {
            foreach (var item in KATDataCFB8.GetGFSBox256BitKey())
            {
                yield return new TestCaseData(item.IV.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox128BitKey()
        {
            foreach (var item in KATDataCFB8.GetKeySBox128BitKey())
            {
                yield return new TestCaseData(item.Key.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox192BitKey()
        {
            foreach (var item in KATDataCFB8.GetKeySBox192BitKey())
            {
                yield return new TestCaseData(item.Key.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox256BitKey()
        {
            foreach (var item in KATDataCFB8.GetKeySBox256BitKey())
            {
                yield return new TestCaseData(item.Key.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt128BitKey()
        {
            foreach (var item in KATDataCFB8.GetVarTxt128BitKey())
            {
                yield return new TestCaseData(item.IV.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt192BitKey()
        {
            foreach (var item in KATDataCFB8.GetVarTxt192BitKey())
            {
                yield return new TestCaseData(item.IV.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt256BitKey()
        {
            foreach (var item in KATDataCFB8.GetVarTxt256BitKey())
            {
                yield return new TestCaseData(item.IV.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey128BitKey()
        {
            foreach (var item in KATDataCFB8.GetVarKey128BitKey())
            {
                yield return new TestCaseData(item.Key.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey192BitKey()
        {
            foreach (var item in KATDataCFB8.GetVarKey192BitKey())
            {
                yield return new TestCaseData(item.Key.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey256BitKey()
        {
            foreach (var item in KATDataCFB8.GetVarKey256BitKey())
            {
                yield return new TestCaseData(item.Key.ToHex(), item);
            }
        }
        #endregion TestData
        
        #region GFSbox
        [Test]
        [TestCaseSource(nameof(_GetGFSBox128BitKey))]
        public void ShouldGFSboxCorrectlyWith128BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), testName);
        }

        [Test]
        [TestCaseSource(nameof(_GetGFSBox192BitKey))]
        public void ShouldGFSboxCorrectlyWith192BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetGFSBox256BitKey))]
        public void ShouldGFSboxCorrectlyWith256BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }
        #endregion GFSbox

        #region KeySBox
        [Test]
        [TestCaseSource(nameof(_GetKeySBox128BitKey))]
        public void ShouldKeySboxCorrectlyWith128BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetKeySBox192BitKey))]
        public void ShouldKeySboxCorrectlyWith192BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetKeySBox256BitKey))]
        public void ShouldKeySboxCorrectlyWith256BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }
        #endregion KeySBox

        #region VarTxt
        [Test]
        [TestCaseSource(nameof(_GetVarTxt128BitKey))]
        public void ShouldVarTxtCorrectlyWith128BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }
        
        [Test]
        [TestCaseSource(nameof(_GetVarTxt192BitKey))]
        public void ShouldVarTxtCorrectlyWith192BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarTxt256BitKey))]
        public void ShouldVarTxtCorrectlyWith256BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }
        #endregion VarTxt

        #region VarKey
        [Test]
        [TestCaseSource(nameof(_GetVarKey128BitKey))]
        public void ShouldVarKeyCorrectlyWith128BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarKey192BitKey))]
        public void ShouldVarKeyCorrectlyWith196BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarKey256BitKey))]
        public void ShouldVarKeyCorrectlyWith256BitKey(string testName, AlgoArrayResponse algoArrayResponse)
        {
            var result = _subject.BlockEncrypt(algoArrayResponse.IV, algoArrayResponse.Key, algoArrayResponse.PlainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText.ToHex(), result.Result.ToHex(), nameof(algoArrayResponse.CipherText));
        }
        #endregion VarKey
    }
}