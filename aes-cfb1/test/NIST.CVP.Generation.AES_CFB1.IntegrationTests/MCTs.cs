using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.IntegrationTests
{
    [TestFixture]
    public class MCTs
    {
        private AES_CFB1_MCT _subject = new AES_CFB1_MCT(new AES_CFB1(new RijndaelFactory(new RijndaelInternals())));

        #region Encrypt
        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            string keyString = "19c27cdff233706c4c6176eb63a4568e";
            string ivString = "30c6e45842a35240331669e7bd3ee1f3";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            var firstExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");

            var secondExpectedKey = new BitString("d00433b558d5e5b504620846ea9335d9");
            var secondExpectedIv = new BitString("c9c64f6aaae695d948037ead89376357");
            var secondExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var secondExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            
            var lastExpectedKey = new BitString("0b677b274bde90ae72844bff9eb0d0b3");
            var lastExpectedIv = new BitString("337584c1b9d72ddb72b0a544b0f41dbf");
            var lastExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var lastExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");


            var result = _subject.MCTEncrypt(iv, key, plainText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.AreEqual(firstExpectedCipherText, firstCipherText, nameof(firstCipherText));
            
            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondPlainText = result.Response[1].PlainText;
            var secondCipherText = result.Response[1].CipherText;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));
            Assert.AreEqual(secondExpectedPlainText, secondPlainText, nameof(secondExpectedPlainText));
            Assert.AreEqual(secondExpectedCipherText, secondCipherText, nameof(secondExpectedCipherText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt192BitKey()
        {
            string keyString = "5f21cf99ae456ed8d65cbc72c0b89759e20cb1091bc69376";
            string ivString = "6e26057d460762ac302a9a64272a2944";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var firstExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");

            var secondExpectedKey = new BitString("579fd63a9d329fa44285e3f834a94747d56c1278a9fc3c3d");
            var secondExpectedIv = new BitString("94d95f8af411d01e3760a371b23aaf4b");

            var lastExpectedKey = new BitString("ffa875bfb283d86a75801112b048bdd4d2f6e5caafbd9994");
            var lastExpectedIv = new BitString("3f597d5af4d9b1a3980cea2b4f60118e");
            var lastExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            var lastExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");


            var result = _subject.MCTEncrypt(iv, key, plainText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.AreEqual(firstExpectedCipherText, firstCipherText, nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt256BitKey()
        {
            string keyString = "299182240c5ff67364c6b1aac2e3f06858f290650c12c359cb47071920c55903";
            string ivString = "5323e8ab1eba7a3212846dabf1493549";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var firstExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");

            var secondExpectedKey = new BitString("ebd07fe190d02504be720ebaf56c963e6375cb3ea01105c9fdfe112f826c9b75");
            var secondExpectedIv = new BitString("3b875b5bac03c69036b91636a2a9c276");

            var lastExpectedKey = new BitString("101dd780875c359a7a4327fe2951e70939db234e72b09c579c12abb01c0f8c1b");
            var lastExpectedIv = new BitString("23028f02b4adc4a5d35ed0e120b142bc");
            var lastExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            var lastExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");


            var result = _subject.MCTEncrypt(iv, key, plainText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.AreEqual(firstExpectedCipherText, firstCipherText, nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        public void ShouldMonteCarloTestDecrypt128BitKey()
        {
            string keyString = "d3057c24774e4420c5dd6d66eeb04912";
            string ivString = "cdb7d2e7a5e85f2b5e6a08d89b2c9bdd";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            var firstExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");

            var secondExpectedKey = new BitString("0679aba4f75ae4e820fd2d2caa297854");
            var secondExpectedIv = new BitString("d57cd7808014a0c8e520404a44993146");
            var secondExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            var secondExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("1");

            var lastExpectedKey = new BitString("d5f29f0400854d31f2ddcdd5c32ce3ba");
            var lastExpectedIv = new BitString("89b57c371d635d67a83aa3ebb4f4a03d");
            var lastExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var lastExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("1");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondCipherText = result.Response[1].CipherText;
            var secondPlainText = result.Response[1].PlainText;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));
            Assert.AreEqual(secondExpectedCipherText, secondCipherText, nameof(secondExpectedCipherText));
            Assert.AreEqual(secondExpectedPlainText, secondPlainText, nameof(secondExpectedPlainText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt192BitKey()
        {
            string keyString = "81e26656c1be3dc9b7275ba3d224bac3fd42c4ba1637688d";
            string ivString = "ebe322635745494100c2b0bbcafc7a33";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            var firstExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");

            var secondExpectedKey = new BitString("e98067eb3a70d4e54fbe0fbd36aa282e41ab0af9a96d2db9");
            var secondExpectedIv = new BitString("f899541ee48e92edbce9ce43bf5a4534");
            var secondExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var secondExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("1");

            var lastExpectedKey = new BitString("481dac4354c7a602f31d9fd33119cd834f69277fabba5c5f");
            var lastExpectedIv = new BitString("40648ac18d340bf6f83887de83cb28db");
            var lastExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1");
            var lastExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondCipherText = result.Response[1].CipherText;
            var secondPlainText = result.Response[1].PlainText;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));
            Assert.AreEqual(secondExpectedCipherText, secondCipherText, nameof(secondExpectedCipherText));
            Assert.AreEqual(secondExpectedPlainText, secondPlainText, nameof(secondExpectedPlainText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt256BitKey()
        {
            string keyString = "63a8a289c2f89f0246e2c42f264ea27013b51e24bf9416c0a01a481ef2246f17";
            string ivString = "6ec27ed42082fb5fb84c27829b03226d";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var firstExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");

            var secondExpectedKey = new BitString("4c1189c6218791ceba937ff30c4c4bee86b03c7569edf1604cd4a816aa8dbaf7");
            var secondExpectedIv = new BitString("95052251d679e7a0eccee00858a9d5e0");
            var secondExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var secondExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("1");

            var lastExpectedKey = new BitString("9ccc8ef62058690c1265f314c752bdcb6c90e7ceaa9ca313a55ff48862005491");
            var lastExpectedIv = new BitString("f2037905c418ed80780ffccdb650bf3c");
            var lastExpectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0");
            var lastExpectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondCipherText = result.Response[1].CipherText;
            var secondPlainText = result.Response[1].PlainText;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));
            Assert.AreEqual(secondExpectedCipherText, secondCipherText, nameof(secondExpectedCipherText));
            Assert.AreEqual(secondExpectedPlainText, secondPlainText, nameof(secondExpectedPlainText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }
        #endregion Decrypt
    }
}
