using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.IntegrationTests
{
    [TestFixture]
    public class MMTs
    {
        private AES_CFB1 _subject =new AES_CFB1(new RijndaelFactory(new RijndaelInternals()));

        [Test]
        public void ShouldMMTEncrypt1Byte128BitKey()
        {
            BitString key = new BitString("2695b44439192d099f0a31b89f24dc0f");
            BitString iv = new BitString("5086c0ed84593ed006919af33d4c8902");
            BitString plainText = BitString.GetBitStringEachCharacterOfInputIsBit("11");
            BitString expectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("11");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        public void ShouldMMTEncryptMultipleBytes128BitKey()
        {
            BitString key = new BitString("7df9fff1c14de3e251aecd3a4328a907");
            BitString iv = new BitString("3903d581031b6b60c0162185bc223fa9");
            BitString plainText = BitString.GetBitStringEachCharacterOfInputIsBit("0100100001");
            BitString expectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0100011111");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        public void ShouldMMTEncrypt1Byte192BitKey()
        {
            BitString key = new BitString("d077cf91d6961cea53cea9e4bad61e1e854b74381f03fdd0");
            BitString iv = new BitString("9a3c967ae97b977e04f2168d7024614f");
            BitString plainText = BitString.GetBitStringEachCharacterOfInputIsBit("01");
            BitString expectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("10");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        public void ShouldMMTEncryptMultipleBytes192BitKey()
        {
            BitString key = new BitString("5ca48a8c95bc37e4b526d5b86da5a7922db2d44b827c6029");
            BitString iv = new BitString("460564e43d949ebe559fc3c230919d5e");
            BitString plainText = BitString.GetBitStringEachCharacterOfInputIsBit("1101100100");
            BitString expectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("0011101110");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        public void ShouldMMTEncrypt1Byte256BitKey()
        {
            BitString key = new BitString("654ba9cf4cfab4a913e85d8ff8aeed7ba2a56730584b61c071e813500b71ae1f");
            BitString iv = new BitString("fc0ed6b5e59f7fe7094697b78d20174e");
            BitString plainText = BitString.GetBitStringEachCharacterOfInputIsBit("11");
            BitString expectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("00");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        public void ShouldMMTEncryptMultipleBytes256BitKey()
        {
            BitString key = new BitString("ec1272869c97608876ac778400d00a11cc59e45b9f0c8d60d1ebc960f74e628e");
            BitString iv = new BitString("61b17b1f9aa2145054d596b4a88e8bbc");
            BitString plainText = BitString.GetBitStringEachCharacterOfInputIsBit("1110110000");
            BitString expectedCipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1000011110");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        //[Test]
        //public void ShouldMMTDecrypt1Byte()
        //{
        //    BitString key = new BitString("83185129c67311b6fc765639b7bb63c0");
        //    BitString iv = new BitString("84324d79ab6adc655d295ee9f8263725");
        //    BitString cipherText = BitString.GetBitStringEachCharacterOfInputIsBit("10");
        //    BitString expectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("01");

        //    var result = _subject.BlockDecrypt(iv, key, cipherText);

        //    Assert.AreEqual(expectedPlainText, result.PlainText);
        //}

        //[Test]
        //public void ShouldMMTDecryptMultipleBytes()
        //{
        //    BitString key = new BitString("2a5e39f2de044223a0fcde0327331602");
        //    BitString iv = new BitString("5e343033f92a3c82efdf9a35ac5e3657");
        //    BitString cipherText = BitString.GetBitStringEachCharacterOfInputIsBit("1001110000");
        //    BitString expectedPlainText = BitString.GetBitStringEachCharacterOfInputIsBit("0000011011");

        //    var result = _subject.BlockDecrypt(iv, key, cipherText);

        //    Assert.AreEqual(expectedPlainText, result.PlainText);
        //}
    }
}
