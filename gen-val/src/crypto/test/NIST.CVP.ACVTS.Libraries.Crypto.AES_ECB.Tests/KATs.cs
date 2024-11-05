using System.Collections;
using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_ECB.Tests
{
    [TestFixture, FastCryptoTest]
    public class KATs
    {
        private readonly EcbBlockCipher _newSubject = new EcbBlockCipher(new AesEngine());

        #region TestData
        static IEnumerable _GetGFSBox128BitKey()
        {
            foreach (var item in KATData.GetGFSBox128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetGFSBox192BitKey()
        {
            foreach (var item in KATData.GetGFSBox192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetGFSBox256BitKey()
        {
            foreach (var item in KATData.GetGFSBox256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox128BitKey()
        {
            foreach (var item in KATData.GetKeySBox128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox192BitKey()
        {
            foreach (var item in KATData.GetKeySBox192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetKeySBox256BitKey()
        {
            foreach (var item in KATData.GetKeySBox256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt128BitKey()
        {
            foreach (var item in KATData.GetVarTxt128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt192BitKey()
        {
            foreach (var item in KATData.GetVarTxt192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarTxt256BitKey()
        {
            foreach (var item in KATData.GetVarTxt256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey128BitKey()
        {
            foreach (var item in KATData.GetVarKey128BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey192BitKey()
        {
            foreach (var item in KATData.GetVarKey192BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        static IEnumerable _GetVarKey256BitKey()
        {
            foreach (var item in KATData.GetVarKey256BitKey())
            {
                yield return new TestCaseData(item.CipherText.ToHex(), item);
            }
        }
        #endregion TestData

        [Test]
        [TestCaseSource(nameof(_GetGFSBox128BitKey))]
        [TestCaseSource(nameof(_GetGFSBox192BitKey))]
        [TestCaseSource(nameof(_GetGFSBox256BitKey))]
        public void ShouldGFSboxCorrectlyNewEngine(string expectedCipherText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.CipherText), expectedCipherText);
        }

        [Test]
        [TestCaseSource(nameof(_GetKeySBox128BitKey))]
        [TestCaseSource(nameof(_GetKeySBox192BitKey))]
        [TestCaseSource(nameof(_GetKeySBox256BitKey))]
        public void ShouldKeySboxCorrectlyNewEngine(string expectedCipherText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.CipherText), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarTxt128BitKey))]
        [TestCaseSource(nameof(_GetVarTxt192BitKey))]
        [TestCaseSource(nameof(_GetVarTxt256BitKey))]
        public void ShouldVarTxtCorrectlyNewEngine(string expectedCipherText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.CipherText), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarKey128BitKey))]
        [TestCaseSource(nameof(_GetVarKey192BitKey))]
        [TestCaseSource(nameof(_GetVarKey256BitKey))]
        public void ShouldVarKeyCorrectlyNewEngine(string expectedCipherText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.CipherText), nameof(algoArrayResponse.CipherText));
        }
    }
}
