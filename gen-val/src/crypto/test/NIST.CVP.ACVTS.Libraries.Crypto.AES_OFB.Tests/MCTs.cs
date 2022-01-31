using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_OFB.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {
        private readonly MonteCarloAesOfb _subject =
            new MonteCarloAesOfb(
                new BlockCipherEngineFactory(),
                new ModeBlockCipherFactory(),
                new AesMonteCarloKeyMaker())
            ;

        #region Encrypt
        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            BitString key = new BitString("b36e1af187632167caf1bd04db427bf5");
            BitString iv = new BitString("a4b61dc718672a8090549cbe5aadb44f");
            BitString plainText = new BitString("2cfd639111e327220f1d2660e1402c3a");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessMonteCarloTest(param);
            var firstExpectedCipherText = new BitString("76156cacb09a34d6d7d9511ad1d8e92e");
            var lastExpectedCipherText = new BitString("11541400b5ef0b588f2ce9df5e74f45e");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.AreEqual(firstExpectedCipherText.ToHex(), firstCipherText, nameof(firstCipherText));
            Assert.AreEqual(lastExpectedCipherText.ToHex(), lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt192BitKey()
        {
            BitString key = new BitString("a8d4bbb2f90bd46df8c1d53f3ffc3c0448178985c9c2399d");
            BitString iv = new BitString("e2144ab30656bbfe92b2eae5bbd6df26");
            BitString plainText = new BitString("f92a1ab2ddf68e13ca40baa9f638a4b9");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedCipherText = new BitString("b9d0ef07ececec4fe34b714b5ee5307d");
            var lastExpectedCipherText = new BitString("1144065d566559e03276ae6cfa5540b5");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.AreEqual(firstExpectedCipherText.ToHex(), firstCipherText, nameof(firstCipherText));
            Assert.AreEqual(lastExpectedCipherText.ToHex(), lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt256BitKey()
        {
            BitString key = new BitString("498a4d6237eddcceeb2867b3e19475d3e3a538e6db70053ce66a07a3a973338b");
            BitString iv = new BitString("252f3d42969df16b8da551260f4a23af");
            BitString plainText = new BitString("d9bc64269d31f9c6dc562a9b70a9483a");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedCipherText = new BitString("32a7689a0b5b628bbb749d3abc0189aa");
            var lastExpectedCipherText = new BitString("10cd655c9034a064158f91634593472a");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.AreEqual(firstExpectedCipherText.ToHex(), firstCipherText, nameof(firstCipherText));
            Assert.AreEqual(lastExpectedCipherText.ToHex(), lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        public void ShouldMonteCarloTestDecrypt128BitKey()
        {
            BitString key = new BitString("fcd8afd687755a906dcb7fed331aeecc");
            BitString iv = new BitString("b57f3da4ba033569a3e4fd0ff36b4bc6");
            BitString cipherText = new BitString("a759240ea1683e275d5b341f2c0dfe5b");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("ad4468da8bb9fa9fe4c8a8b395f262e8");
            var lastExpectedPlainText = new BitString("6d4b5b96d9bae9a2ac0aac92ee666a20");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.AreEqual(firstExpectedPlainText.ToHex(), firstPlaintText, nameof(firstExpectedPlainText));
            Assert.AreEqual(lastExpectedPlainText.ToHex(), lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt192BitKey()
        {
            BitString key = new BitString("0f0805c4127c6f747d0a99ba9a233f371906b3c105e376aa");
            BitString iv = new BitString("88e19bcb11e9d566a63de651cb5c1d2e");
            BitString cipherText = new BitString("03dcc1f22dc222d4b9c75a9f44a5850b");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("ff717172df5b344777aede8919f716b2");
            var lastExpectedPlainText = new BitString("f3ae7bc4987f45a50d11c5a61e42cd79");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.AreEqual(firstExpectedPlainText.ToHex(), firstPlaintText, nameof(firstExpectedPlainText));
            Assert.AreEqual(lastExpectedPlainText.ToHex(), lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt256BitKey()
        {
            BitString key = new BitString("82f686042a40193035be076ea23e759dea24c4eb93b2c23b028621cb91642813");
            BitString iv = new BitString("ec5acfa04ae86e2cb4c1d84d745cbbcb");
            BitString cipherText = new BitString("340a0a7715f8c9d46af32cfa526e5eb0");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("5ce78446e7a266fd9312ecb2011d2d9f");
            var lastExpectedPlainText = new BitString("7f3df663c831a78f9931ad580d2a75df");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.AreEqual(firstExpectedPlainText.ToHex(), firstPlaintText, nameof(firstExpectedPlainText));
            Assert.AreEqual(lastExpectedPlainText.ToHex(), lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }
        #endregion Decrypt
    }
}
