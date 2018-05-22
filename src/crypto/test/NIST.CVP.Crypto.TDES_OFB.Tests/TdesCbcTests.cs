using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_OFB.Tests
{
    [TestFixture, FastCryptoTest]
    public class TdesOfbTests
    {
        private readonly OfbBlockCipher _newSubject = new OfbBlockCipher(new TdesEngine());
        
        [Test]
        public void ShouldEncrypt()
        {
            var key = new BitString("0101010101010101");
            var plainText = new BitString("0000000000000000");
            var expectedCipherText = new BitString("95f8a5e5dd31d900");
            var iv = new BitString("8000000000000000");
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(key, plainText, iv);

            Assert.IsTrue((bool)result.Success);
            Assert.AreEqual(expectedCipherText.ToHex(), result.Result.ToHex());
        }

        [Test]
        public void ShouldDecrypt()
        {
            var key = new BitString("0101010101010101");
            var expectedPlainText = new BitString("0000000000000000");
            var cipherText = new BitString("95f8a5e5dd31d900");
            var iv = new BitString("8000000000000000");
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(key, cipherText, iv);

            Assert.IsTrue((bool)result.Success);
            Assert.AreEqual(expectedPlainText.ToHex(), result.Result.ToHex());
        }

        [Test]
        public void ShouldFailEncrypt()
        {
            var keyBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            var PlainTextBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a };
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(new BitString(keyBytes), new BitString(PlainTextBytes), new BitString(iv));
            Assert.IsFalse((bool)result.Success);
            Assert.IsNotNull(result.ErrorMessage);
        }

        public void ShouldFailDecrypt()
        {
            var keyBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            var CipherTextBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a };
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(new BitString(keyBytes), new BitString(CipherTextBytes), new BitString(iv));
            Assert.IsFalse((bool)result.Success);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        [TestCase("2fd5fb2aea3462a120e6cd5b20f2bc8ff48a9e58f1cb89d5", "1018c71607c83787", "bb68f8f6a0a2c749", "95a87d4e54522958")]
        [TestCase("b9e0e032375ebae04cda7cab1c3d2f5119d346f208ea83f7", "b8eab3ef61c7d1b4", "b78e05dc12ecdfb59291c3cd3bff7dd2", "070dbff0d1333afe81f84ca3510f0ce2")]
        public void ShouldPassMultiBlockEncryptTests(string key, string iv, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(cipherText).ToHex(), result.Result.ToHex());
        }

        [Test]
        [TestCase("198f973b5462df64946245e0b6ec0d98a4e5136ba75294b3", "60dd7b8f00554bb6003161ba62fd66adc2757a40dbf5d6d84d30b0e49e255f6d7e4fc0ee4f1cd867e4a1bbad898cac04445a85ef5bca5a471691598bc64ff47706c243d84139a39a", "b141cecac08330b54c182e9115881265693cd5a84e505c903dd81757fc351dfcc6ecdd23672d24761c7cb05978abc45dd4a5dc34ae11d65eaa02606aac3bffc76f5d6c4a7d5d17b0")]
        [TestCase("c1da626de38389f11cc28fe0f8fdb623374ccb495d20c81f", "5fe074a3c30b281ec7db62b76ddbcb7d51c784242ffbc410a42ac2b03953d50d9df1d9a33273d66fcbebdc49b50a3174f44caf74ce70671f8e2b8af7821d8ab746047c2c4430c1467c37e56f81e9c71c", "d7f5edf35f207dfcdae580d4d903a3c289da22a9ec1c0e06ffd333f47773d3193570cc048571b923d21a1871eb228f4155455535da42e71289764000356acf089605a5f57aba25533bda531db14f6553")]
        public void ShouldFailMultiBlockEncryptTests(string key, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockEncrypt(new BitString(key), new BitString(iv), new BitString(plaintext));
            Assert.IsTrue((bool)result.Success);

            Assert.AreNotEqual(new BitString(cipherText), result.Result);
        }

        [Test]
        [TestCase("3e37e6fd7f15a7eff7f2ce83759bae97296e4fbfabe91025", "136aa27207e95411", "54a3be1f0886d8e1", "4b3581305873de1b")]
        [TestCase("4c9229576b04ad837fa1513804c215ae9efd9e38e3a8765b", "47302567b055db1d", "e513b748ea166b1328533e2879838002e060f7fc0bfc18e2", "548f67b8e6ea5ef313085d30f1f2546f0e515a38618d44de")]
        public void ShouldPassMultiBlockDecryptTests(string key, string iv, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(new BitString(key), new BitString(cipherText), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.Result.ToHex());
        }


        [Test]
        public void ShouldEncryptNewEngine()
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
            var result = _newSubject.ProcessPayload(param);
            
            Assert.IsTrue((bool)result.Success);
            Assert.AreEqual(expectedCipherText.ToHex(), result.Result.ToHex());
        }

        [Test]
        public void ShouldDecryptNewEngine()
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
            var result = _newSubject.ProcessPayload(param);
            
            Assert.IsTrue((bool)result.Success);
            Assert.AreEqual(expectedPlainText.ToHex(), result.Result.ToHex());
        }
        
        [Test]
        [TestCase("2fd5fb2aea3462a120e6cd5b20f2bc8ff48a9e58f1cb89d5", "1018c71607c83787", "bb68f8f6a0a2c749", "95a87d4e54522958")]
        [TestCase("7345ba45dab6b0c89b7fabc4083b9db626b00757e961578f", "a3b5bc66da13dd92", "7abc20f1798eb94fbb8f3a455c6f3b804c3e15e16a965817f6bbbff44b73cc4f12325bc333b5591843b80ebce65142ccd21be5d5b6f8bc60298cffd6e56d3bd5950e6271356925c789dd24619ea6607c", "cd9d344ba239554bb7fa4bfe1e80c5a722b8902c205c0dbc99b9e3b484b5f1c4e910874eabead50ea686a300f94d437ea516c4a958a4690b8370597bb2a998401d7a36797a8bc7bc16ca13fd246b0eb1")]
        public void ShouldPassMultiBlockEncryptTestsNewEngine(string key, string iv, string plaintext, string cipherText)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                new BitString(iv),
                new BitString(key),
                new BitString(plaintext)
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue((bool)result.Success);
            Assert.AreEqual(new BitString(cipherText).ToHex(), result.Result.ToHex());
        }
        
        [Test]
        [TestCase("3e37e6fd7f15a7eff7f2ce83759bae97296e4fbfabe91025", "136aa27207e95411", "54a3be1f0886d8e1", "4b3581305873de1b")]
        [TestCase("7345ba45dab6b0c89b7fabc4083b9db626b00757e961578f", "a3b5bc66da13dd92", "7abc20f1798eb94fbb8f3a455c6f3b804c3e15e16a965817f6bbbff44b73cc4f12325bc333b5591843b80ebce65142ccd21be5d5b6f8bc60298cffd6e56d3bd5950e6271356925c789dd24619ea6607c", "cd9d344ba239554bb7fa4bfe1e80c5a722b8902c205c0dbc99b9e3b484b5f1c4e910874eabead50ea686a300f94d437ea516c4a958a4690b8370597bb2a998401d7a36797a8bc7bc16ca13fd246b0eb1")]
        public void ShouldPassMultiBlockDecryptTestsNewEngine(string key, string iv, string plaintext, string cipherText)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                new BitString(iv),
                new BitString(key),
                new BitString(cipherText)
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.Result.ToHex());
        }
    }
}
