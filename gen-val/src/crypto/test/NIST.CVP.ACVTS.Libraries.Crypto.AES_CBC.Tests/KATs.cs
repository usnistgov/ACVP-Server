﻿using System.Collections;
using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CBC.Tests
{
    [TestFixture, FastCryptoTest]
    public class KATs
    {
        private readonly CbcBlockCipher _newSubject = new CbcBlockCipher(new AesEngine());

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
        public void ShouldGFSboxCorrectlyEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.CipherText), expectedText);
        }

        [Test]
        [TestCaseSource(nameof(_GetKeySBox128BitKey))]
        [TestCaseSource(nameof(_GetKeySBox192BitKey))]
        [TestCaseSource(nameof(_GetKeySBox256BitKey))]
        public void ShouldKeySboxCorrectlyEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
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
        public void ShouldVarTxtCorrectlyEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
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
        public void ShouldVarKeyCorrectlyEncrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.PlainText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.CipherText), nameof(algoArrayResponse.CipherText));
        }


        [Test]
        [TestCaseSource(nameof(_GetGFSBox128BitKey))]
        [TestCaseSource(nameof(_GetGFSBox192BitKey))]
        [TestCaseSource(nameof(_GetGFSBox256BitKey))]
        public void ShouldGFSboxCorrectlyDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.PlainText), expectedText);
        }

        [Test]
        [TestCaseSource(nameof(_GetKeySBox128BitKey))]
        [TestCaseSource(nameof(_GetKeySBox192BitKey))]
        [TestCaseSource(nameof(_GetKeySBox256BitKey))]
        public void ShouldKeySboxCorrectlyDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.PlainText), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarTxt128BitKey))]
        [TestCaseSource(nameof(_GetVarTxt192BitKey))]
        [TestCaseSource(nameof(_GetVarTxt256BitKey))]
        public void ShouldVarTxtCorrectlyDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.PlainText), nameof(algoArrayResponse.CipherText));
        }

        [Test]
        [TestCaseSource(nameof(_GetVarKey128BitKey))]
        [TestCaseSource(nameof(_GetVarKey192BitKey))]
        [TestCaseSource(nameof(_GetVarKey256BitKey))]
        public void ShouldVarKeyCorrectlyDecrypt(string expectedText, AlgoArrayResponse algoArrayResponse)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                algoArrayResponse.IV,
                algoArrayResponse.Key,
                algoArrayResponse.CipherText
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            Assert.That(result.Result, Is.EqualTo(algoArrayResponse.PlainText), nameof(algoArrayResponse.CipherText));
        }
    }
}
