using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB8;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB8.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTs
    {
        AES_CFB8_MCT _subject = new AES_CFB8_MCT(
            new Crypto.AES_CFB8.AES_CFB8(
                new RijndaelFactory(
                    new RijndaelInternals()
                )
            )
        );

        #region Encrypt
        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            string keyString = "be2742df6d0139a45eb0e9a1822eef4a";
            string ivString = "3d3d13c3096535cfd1889e592308f9f2";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("8d");
            var firstExpectedCipherText = new BitString("8a");

            var secondExpectedKey = new BitString("0078ef5381ec8f98b21ff995e1f058c0");
            var secondExpectedIv = new BitString("be5fad8cecedb63cecaf103463deb78a");
            var secondExpectedPlainText = new BitString("26");
            var secondExpectedCipherText = new BitString("ff");

            var lastExpectedKey = new BitString("2ba452ed17c06b9602fd6a80f3112c38");
            var lastExpectedIv = new BitString("b0b335f8827ecd6e2afe15b5b87a8185");
            var lastExpectedPlainText = new BitString("6f");
            var lastExpectedCipherText = new BitString("77");


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
            string keyString = "71e5ad1af5ae95d5a63575177b22a6e43ab9a6d0f454bd14";
            string ivString = "233aef115929ac7c6e225ddb4509a159";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("72");
            var firstExpectedCipherText = new BitString("e7");

            var secondExpectedKey = new BitString("ae29cd48bda44fa12f887dda1d75f769b7665d53329da0f3");
            var secondExpectedIv = new BitString("89bd08cd6657518d8ddffb83c6c91de7");

            var lastExpectedKey = new BitString("cda2456926db47a89dde7652df96a1f4752ad6f5db5bd5ad");
            var lastExpectedIv = new BitString("5fb0d1b4a9b886e627013bec9a9debd6");
            var lastExpectedPlainText = new BitString("2f");
            var lastExpectedCipherText = new BitString("df");


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
            string keyString = "33f9810a019abf3031639e28cd441e7f7d54c92cab68f2c5e6e43bf384d15a24";
            string ivString = "47b2e9ceb89bbde5f0b65e6a6c604a2d";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("b0");
            var firstExpectedCipherText = new BitString("f8");

            var secondExpectedKey = new BitString("29f81b5019fcf708c4228fbdd990b3e1d96018d539910169b5562c2781ffbadc");
            var secondExpectedIv = new BitString("a434d1f992f9f3ac53b217d4052ee0f8");

            var lastExpectedKey = new BitString("07abdfa3ef9c70115f653f858f121501b1f7612544659461d2b85ba7567f035b");
            var lastExpectedIv = new BitString("978a1ec1ad1b1ab6fafd531e56709f57");
            var lastExpectedPlainText = new BitString("77");
            var lastExpectedCipherText = new BitString("c3");


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
            string keyString = "b8aa42a5a808ddefb65f9da4f7bb69e0";
            string ivString = "0d5efe13e859a98483027fc5acfd6cac";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString("aa");
            var firstExpectedPlainText = new BitString("81");

            var secondExpectedKey = new BitString("b8f6c6ffe1d3f522cda5246349d23a61");
            var secondExpectedIv = new BitString("005c845a49db28cd7bfab9c7be695381");

            var lastExpectedKey = new BitString("665f12932939fb8c20c9a13e7384a0c3");
            var lastExpectedIv = new BitString("47dd8c14ce80295cbb0d52ad055ea3e7");
            var lastExpectedCipherText = new BitString("bb");
            var lastExpectedPlainText = new BitString("87");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

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
            string keyString = "7f1d2d31bbcb3cf9c4cd26b07e4412345ca5372e797693f1";
            string ivString = "870450cf1c083d5fa7f7039a90a6f025";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString("9e");
            var firstExpectedPlainText = new BitString("05");

            var secondExpectedKey = new BitString("c93e5a2905c61bbfef4ce757a599f5738b859132bdbc03f4");
            var secondExpectedIv = new BitString("2b81c1e7dbdde747d720a61cc4ca9005");

            var lastExpectedKey = new BitString("6c6dc9583bd02e3ae69e61a7c1c2ccd6929fbe0c4d5047be");
            var lastExpectedIv = new BitString("24eb41d5dcf09dd4e88a84f521f89b8d");
            var lastExpectedCipherText = new BitString("dd");
            var lastExpectedPlainText = new BitString("b7");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

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
            string keyString = "18c80d340c0d1f96571395e0b87137076162e4bd4ac98694cb7245656a9bfbf3";
            string ivString = "f44586998c1d5658fffa686c25998f2b";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString("02");
            var firstExpectedPlainText = new BitString("30");

            var secondExpectedKey = new BitString("118fd22241f56fdab7ce59c06660a2f9fcd5071842b4fea1add02ca2308f42c3");
            var secondExpectedIv = new BitString("9db7e3a5087d783566a269c75a14b930");

            var lastExpectedKey = new BitString("0ed49def9a58dab52c2804dad5677fc94e0ef456650bceb1a562146f91c41654");
            var lastExpectedIv = new BitString("4b1d66e0c04522c80b048307dcc89dda");
            var lastExpectedCipherText = new BitString("94");
            var lastExpectedPlainText = new BitString("51");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

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
