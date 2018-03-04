using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_ECB.Tests
{
    [TestFixture, FastCryptoTest]
    public class MonteCarloKeyMakerTests
    {
        private Random800_90 _randy = new Random800_90();

        [Test]
        public void ShouldThrowAnExceptionIfNotEnoughCiphersSupplied()
        {
            var subject = new MonteCarloKeyMaker();
            Assert.Throws(
               typeof(ArgumentException),
               () => subject.MixKeys(new TDESKeys(new BitString(64)), new List<BitString>())
           );
        }

        [Test]
        public void ShouldThrowAnExceptionIfNullCiphersSupplied()
        {
            var subject = new MonteCarloKeyMaker();
            Assert.Throws(
               typeof(ArgumentException),
               () => subject.MixKeys(new TDESKeys(new BitString(64)), null)
           );
        }

        [Test]
        public void ShouldThrowAnExceptionIfNullKeysSupplied()
        {
            var subject = new MonteCarloKeyMaker();
            Assert.Throws(
               typeof(ArgumentNullException),
               () => subject.MixKeys(null, new List<BitString> {new BitString(64), new BitString(64), new BitString(64)})
           );
        }

        [Test]
        [TestCase("0123456789ABCDEF")]
        [TestCase("ABCDEF0123456789")]
        public void ShouldKeepAllKeysIdentical(string keyHex)
        {
            var key = new BitString(keyHex);
            var subject = new MonteCarloKeyMaker();
            var result = subject.MixKeys(new TDESKeys(key), GetCipherList());
            var resultKeys = new TDESKeys(result);
            Assert.IsTrue(resultKeys.KeysAsBitStrings.All(k => k.Equals(resultKeys.KeysAsBitStrings[0])));
        }


        [Test]
        [TestCase("0123456789ABCDEFABCDEF0123456789")]
        [TestCase("ABCDEF01234567890123456789ABCDEF")]
        public void ShouldKeepKeys1And3Identical(string keyHex)
        {
            var key = new BitString(keyHex);
            var subject = new MonteCarloKeyMaker();
            var result = subject.MixKeys(new TDESKeys(key), GetCipherList());
            var resultKeys = new TDESKeys(result);
            Assert.AreEqual(resultKeys.KeysAsBitStrings[0], resultKeys.KeysAsBitStrings[2]);
        }


        [Test]
        [TestCase("0123456789ABCDEF")]
        [TestCase("ABCDEF0123456789")]
        public void ShouldUseCipher0ForXORForAllKeys(string keyHex)
        {
            var key = new BitString(keyHex);
            var subject = new MonteCarloKeyMaker();
            var tdesKeys = new TDESKeys(key);
            var lastThreeCipherTexts = GetCipherList();
            var result = subject.MixKeys(tdesKeys, lastThreeCipherTexts);
            var resultKeys = new TDESKeys(result);
            for(int keyIdx = 0; keyIdx < 3; keyIdx++)
            {
                var tdesKey = tdesKeys.KeysAsBitStrings[keyIdx];
                var xorResult = tdesKey.XOR(lastThreeCipherTexts[0]);
                Assert.AreEqual(xorResult, resultKeys.KeysAsBitStrings[keyIdx]);
            }
        }

        [Test]
        [TestCase("0123456789ABCDEFABCDEF0123456789")]
        [TestCase("ABCDEF01234567890123456789ABCDEF")]
        public void ShouldUseCipher0ForKeys1And3(string keyHex)
        {
            var key = new BitString(keyHex);
            var subject = new MonteCarloKeyMaker();
            var tdesKeys = new TDESKeys(key);
            var lastThreeCipherTexts = GetCipherList();
            var result = subject.MixKeys(tdesKeys, lastThreeCipherTexts);
            var resultKeys = new TDESKeys(result);
            for (int keyIdx = 0; keyIdx < 3; keyIdx+=2)
            {
                var tdesKey = tdesKeys.KeysAsBitStrings[keyIdx];
                var xorResult = tdesKey.XOR(lastThreeCipherTexts[0]);
                Assert.AreEqual(xorResult, resultKeys.KeysAsBitStrings[keyIdx]);
            }
        }

        [Test]
        [TestCase("0123456789ABCDEFABCDEF0123456789")]
        [TestCase("ABCDEF01234567890123456789ABCDEF")]
        public void ShouldUseCipher1ForKey2(string keyHex)
        {
            var key = new BitString(keyHex);
            var subject = new MonteCarloKeyMaker();
            var tdesKeys = new TDESKeys(key);
            var lastThreeCipherTexts = GetCipherList();
            var result = subject.MixKeys(tdesKeys, lastThreeCipherTexts);
            var resultKeys = new TDESKeys(result);
       
            var tdesKey = tdesKeys.KeysAsBitStrings[1];
            var xorResult = tdesKey.XOR(lastThreeCipherTexts[1]);
            Assert.AreEqual(xorResult, resultKeys.KeysAsBitStrings[1]);
           
        }

        [Test]
        [TestCase("0123456789ABCDEFABCDEF0123456789A0B1C2D3E4F56789")]
        [TestCase("ABCDEF01234567890123456789ABCDEFE4F56789A0B1C2D3")]
        public void ShouldUseCorrespondingCipherForAllKeys(string keyHex)
        {
            var key = new BitString(keyHex);
            var subject = new MonteCarloKeyMaker();
            var tdesKeys = new TDESKeys(key);
            var lastThreeCipherTexts = GetCipherList();
            var result = subject.MixKeys(tdesKeys, lastThreeCipherTexts);
            var resultKeys = new TDESKeys(result);
            for (int keyIdx = 0; keyIdx < 3; keyIdx++)
            {
                var tdesKey = tdesKeys.KeysAsBitStrings[keyIdx];
                var xorResult = tdesKey.XOR(lastThreeCipherTexts[keyIdx]);
                Assert.AreEqual(xorResult, resultKeys.KeysAsBitStrings[keyIdx]);
            }
        }


        private List<BitString> GetCipherList()
        {
            var list = new List<BitString>();
            list.Add(_randy.GetRandomBitString(64));
            list.Add(_randy.GetRandomBitString(64));
            list.Add(_randy.GetRandomBitString(64));
            return list;
        }
    }
}

