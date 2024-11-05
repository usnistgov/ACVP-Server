using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES_OFB.Tests
{
    [TestFixture, FastCryptoTest]
    public class TdesOfbTests
    {
        private readonly OfbBlockCipher _subject = new OfbBlockCipher(new TdesEngine());

        [Test]
        public void ShouldEncrypt()
        {
            var key = new BitString("0101010101010101");
            var plainText = new BitString("0000000000000000");
            var expectedCipherText = new BitString("95f8a5e5dd31d900");
            var iv = new BitString("8000000000000000");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessPayload(param);

            Assert.That((bool)result.Success, Is.True);
            Assert.That(result.Result.ToHex(), Is.EqualTo(expectedCipherText.ToHex()));
        }

        [Test]
        public void ShouldDecrypt()
        {
            var key = new BitString("0101010101010101");
            var expectedPlainText = new BitString("0000000000000000");
            var cipherText = new BitString("95f8a5e5dd31d900");
            var iv = new BitString("8000000000000000");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessPayload(param);

            Assert.That((bool)result.Success, Is.True);
            Assert.That(result.Result.ToHex(), Is.EqualTo(expectedPlainText.ToHex()));
        }

        [Test]
        [TestCase("2fd5fb2aea3462a120e6cd5b20f2bc8ff48a9e58f1cb89d5", "1018c71607c83787", "bb68f8f6a0a2c749", "95a87d4e54522958")]
        [TestCase("7345ba45dab6b0c89b7fabc4083b9db626b00757e961578f", "a3b5bc66da13dd92", "7abc20f1798eb94fbb8f3a455c6f3b804c3e15e16a965817f6bbbff44b73cc4f12325bc333b5591843b80ebce65142ccd21be5d5b6f8bc60298cffd6e56d3bd5950e6271356925c789dd24619ea6607c", "cd9d344ba239554bb7fa4bfe1e80c5a722b8902c205c0dbc99b9e3b484b5f1c4e910874eabead50ea686a300f94d437ea516c4a958a4690b8370597bb2a998401d7a36797a8bc7bc16ca13fd246b0eb1")]
        public void ShouldPassMultiBlockEncryptTests(string key, string iv, string plaintext, string cipherText)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                new BitString(iv),
                new BitString(key),
                new BitString(plaintext)
            );
            var result = _subject.ProcessPayload(param);

            Assert.That((bool)result.Success, Is.True);
            Assert.That(result.Result.ToHex(), Is.EqualTo(new BitString(cipherText).ToHex()));
        }

        [Test]
        [TestCase("3e37e6fd7f15a7eff7f2ce83759bae97296e4fbfabe91025", "136aa27207e95411", "54a3be1f0886d8e1", "4b3581305873de1b")]
        [TestCase("7345ba45dab6b0c89b7fabc4083b9db626b00757e961578f", "a3b5bc66da13dd92", "7abc20f1798eb94fbb8f3a455c6f3b804c3e15e16a965817f6bbbff44b73cc4f12325bc333b5591843b80ebce65142ccd21be5d5b6f8bc60298cffd6e56d3bd5950e6271356925c789dd24619ea6607c", "cd9d344ba239554bb7fa4bfe1e80c5a722b8902c205c0dbc99b9e3b484b5f1c4e910874eabead50ea686a300f94d437ea516c4a958a4690b8370597bb2a998401d7a36797a8bc7bc16ca13fd246b0eb1")]
        public void ShouldPassMultiBlockDecryptTests(string key, string iv, string plaintext, string cipherText)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                new BitString(iv),
                new BitString(key),
                new BitString(cipherText)
            );
            var result = _subject.ProcessPayload(param);

            Assert.That((bool)result.Success, Is.True);

            Assert.That(result.Result.ToHex(), Is.EqualTo(new BitString(plaintext).ToHex()));
        }
    }
}
