using System.Collections;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_OFB.Tests
{
    [TestFixture, FastCryptoTest]
    public class KATs
    {
        private OfbBlockCipher _newSubject = new OfbBlockCipher(new AesEngine());

        #region TestData
        static IEnumerable _GetGFSBox128BitKey()
        {
            foreach (var item in KATDataOFB.GetGFSBox128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetGFSBox192BitKey()
        {
            foreach (var item in KATDataOFB.GetGFSBox192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetGFSBox256BitKey()
        {
            foreach (var item in KATDataOFB.GetGFSBox256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox128BitKey()
        {
            foreach (var item in KATDataOFB.GetKeySBox128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox192BitKey()
        {
            foreach (var item in KATDataOFB.GetKeySBox192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox256BitKey()
        {
            foreach (var item in KATDataOFB.GetKeySBox256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt128BitKey()
        {
            foreach (var item in KATDataOFB.GetVarTxt128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt192BitKey()
        {
            foreach (var item in KATDataOFB.GetVarTxt192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt256BitKey()
        {
            foreach (var item in KATDataOFB.GetVarTxt256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey128BitKey()
        {
            foreach (var item in KATDataOFB.GetVarKey128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey192BitKey()
        {
            foreach (var item in KATDataOFB.GetVarKey192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey256BitKey()
        {
            foreach (var item in KATDataOFB.GetVarKey256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        #endregion TestData

        [Test]
        [TestCaseSource(nameof(_GetGFSBox128BitKey))]
        [TestCaseSource(nameof(_GetGFSBox192BitKey))]
        [TestCaseSource(nameof(_GetGFSBox256BitKey))]
        public void ShouldGFSboxCorrectlyNewEngineEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText, result.Result, expectedText);
        }

        [Test]
        [TestCaseSource(nameof(_GetKeySBox128BitKey))]
        [TestCaseSource(nameof(_GetKeySBox192BitKey))]
        [TestCaseSource(nameof(_GetKeySBox256BitKey))]
        public void ShouldKeySboxCorrectlyNewEngineEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText, result.Result, nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarTxt128BitKey))]
        [TestCaseSource(nameof(_GetVarTxt192BitKey))]
        [TestCaseSource(nameof(_GetVarTxt256BitKey))]
        public void ShouldVarTxtCorrectlyNewEngineEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText, result.Result, nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarKey128BitKey))]
        [TestCaseSource(nameof(_GetVarKey192BitKey))]
        [TestCaseSource(nameof(_GetVarKey256BitKey))]
        public void ShouldVarKeyCorrectlyNewEngineEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.CipherText, result.Result, nameof(algoArrayResponse.CipherText));
        }


        [Test]
        [TestCaseSource(nameof(_GetGFSBox128BitKey))]
        [TestCaseSource(nameof(_GetGFSBox192BitKey))]
        [TestCaseSource(nameof(_GetGFSBox256BitKey))]
        public void ShouldGFSboxCorrectlyNewEngineDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.PlainText, result.Result, expectedText);
        }

        [Test]
        [TestCaseSource(nameof(_GetKeySBox128BitKey))]
        [TestCaseSource(nameof(_GetKeySBox192BitKey))]
        [TestCaseSource(nameof(_GetKeySBox256BitKey))]
        public void ShouldKeySboxCorrectlyNewEngineDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.PlainText, result.Result, nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarTxt128BitKey))]
        [TestCaseSource(nameof(_GetVarTxt192BitKey))]
        [TestCaseSource(nameof(_GetVarTxt256BitKey))]
        public void ShouldVarTxtCorrectlyNewEngineDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.PlainText, result.Result, nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarKey128BitKey))]
        [TestCaseSource(nameof(_GetVarKey192BitKey))]
        [TestCaseSource(nameof(_GetVarKey256BitKey))]
        public void ShouldVarKeyCorrectlyNewEngineDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(algoArrayResponse.PlainText, result.Result, nameof(algoArrayResponse.CipherText));
        }
    }
}
