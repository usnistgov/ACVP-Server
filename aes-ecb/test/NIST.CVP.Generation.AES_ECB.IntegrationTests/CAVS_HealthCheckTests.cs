using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.IntegrationTests
{
    [TestFixture]
    public class CAVS_HealthCheckTests
    {

        private Crypto.AES_ECB.AES_ECB _subject = new Crypto.AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals()));

        [Test]
        public void ShouldEncryptWith128BitKey()
        {
            
            byte[] keyBytes = new byte[128/8];
            BitString keyBits = new BitString(keyBytes);

            byte[] plainText = new byte[16];

            plainText[0] = 0xf3;
            plainText[1] = 0x44;
            plainText[2] = 0x81;
            plainText[3] = 0xec;
            plainText[4] = 0x3c;
            plainText[5] = 0xc6;
            plainText[6] = 0x27;
            plainText[7] = 0xba;
            plainText[8] = 0xcd;
            plainText[9] = 0x5d;
            plainText[10] = 0xc3;
            plainText[11] = 0xfb;
            plainText[12] = 0x08;
            plainText[13] = 0xf2;
            plainText[14] = 0x73;
            plainText[15] = 0xe6;

            BitString plainTextBits = new BitString(plainText);

            var encryptOperation = _subject.BlockEncrypt(keyBits, plainTextBits);
            Assert.IsTrue(encryptOperation.Success, nameof(encryptOperation));
            var ct = encryptOperation.CipherText.ToBytes();
            if ((ct[0] != 0x03) || (ct[1] != 0x36) || (ct[2] != 0x76) || (ct[3] != 0x3e) ||
                (ct[4] != 0x96) || (ct[5] != 0x6d) || (ct[6] != 0x92) || (ct[7] != 0x59) ||
                (ct[8] != 0x5a) || (ct[9] != 0x56) || (ct[10] != 0x7c) || (ct[11] != 0xc9) ||
                (ct[12] != 0xce) || (ct[13] != 0x53) || (ct[14] != 0x7f) || (ct[15] != 0x5e))
                Assert.Fail("Invalid cipher");

            // Do the decryption to ensure we arrive back at plain text
            var decryptOperation = _subject.BlockDecrypt(keyBits, encryptOperation.CipherText);
            var pt = decryptOperation.PlainText.ToBytes();
            for (int i = 0; i < pt.Length; i++)
            {
                Assert.AreEqual(plainText[i], pt[i], $"Failed on index {i}");
            }

            Assert.Pass();
        }

        [Test]
        public void ShouldEncryptWith192BitKey()
        {

            byte[] keyBytes = new byte[192 / 8];
            BitString keyBits = new BitString(keyBytes);

            byte[] plainText = new byte[16];

            // PT
            plainText[0] = 0x1b;
            plainText[1] = 0x07;
            plainText[2] = 0x7a;
            plainText[3] = 0x6a;
            plainText[4] = 0xf4;
            plainText[5] = 0xb7;
            plainText[6] = 0xf9;
            plainText[7] = 0x82;
            plainText[8] = 0x29;
            plainText[9] = 0xde;
            plainText[10] = 0x78;
            plainText[11] = 0x6d;
            plainText[12] = 0x75;
            plainText[13] = 0x16;
            plainText[14] = 0xb6;
            plainText[15] = 0x39;

            BitString plainTextBits = new BitString(plainText);

            var encryptOperation = _subject.BlockEncrypt(keyBits, plainTextBits);

            Assert.IsTrue(encryptOperation.Success, nameof(encryptOperation));

            var ct = encryptOperation.CipherText.ToBytes();

            if ((ct[0] != 0x27) || (ct[1] != 0x5c) || (ct[2] != 0xfc) || (ct[3] != 0x04) ||
                (ct[4] != 0x13) || (ct[5] != 0xd8) || (ct[6] != 0xcc) || (ct[7] != 0xb7) ||
                (ct[8] != 0x05) || (ct[9] != 0x13) || (ct[10] != 0xc3) || (ct[11] != 0x85) ||
                (ct[12] != 0x9b) || (ct[13] != 0x1d) || (ct[14] != 0x0f) || (ct[15] != 0x72))
                Assert.Fail("Invalid cipher");

            // Do the decryption to ensure we arrive back at plain text
            var decryptOperation = _subject.BlockDecrypt(keyBits, encryptOperation.CipherText);
            var pt = decryptOperation.PlainText.ToBytes();
            for (int i = 0; i < pt.Length; i++)
            {
                Assert.AreEqual(plainText[i], pt[i], $"Failed on index {i}");
            }

            Assert.Pass();
        }

        [Test]
        public void ShouldEncryptWith256BitKey()
        {

            byte[] keyBytes = new byte[256 / 8];
            BitString keyBits = new BitString(keyBytes);

            byte[] plainText = new byte[16];

            plainText[0] = 0x01;
            plainText[1] = 0x47;
            plainText[2] = 0x30;
            plainText[3] = 0xf8;
            plainText[4] = 0x0a;
            plainText[5] = 0xc6;
            plainText[6] = 0x25;
            plainText[7] = 0xfe;
            plainText[8] = 0x84;
            plainText[9] = 0xf0;
            plainText[10] = 0x26;
            plainText[11] = 0xc6;
            plainText[12] = 0x0b;
            plainText[13] = 0xfd;
            plainText[14] = 0x54;
            plainText[15] = 0x7d;

            BitString plainTextBits = new BitString(plainText);

            var encryptOperation = _subject.BlockEncrypt(keyBits, plainTextBits);

            Assert.IsTrue(encryptOperation.Success, nameof(encryptOperation));

            var ct = encryptOperation.CipherText.ToBytes();

            if ((ct[0] != 0x5c) || (ct[1] != 0x9d) || (ct[2] != 0x84) || (ct[3] != 0x4e) ||
                (ct[4] != 0xd4) || (ct[5] != 0x6f) || (ct[6] != 0x98) || (ct[7] != 0x85) ||
                (ct[8] != 0x08) || (ct[9] != 0x5e) || (ct[10] != 0x5d) || (ct[11] != 0x6a) ||
                (ct[12] != 0x4f) || (ct[13] != 0x94) || (ct[14] != 0xc7) || (ct[15] != 0xd7))
                Assert.Fail("Invalid cipher");

            // Do the decryption to ensure we arrive back at plain text
            var decryptOperation = _subject.BlockDecrypt(keyBits, encryptOperation.CipherText);
            var pt = decryptOperation.PlainText.ToBytes();
            for (int i = 0; i < pt.Length; i++)
            {
                Assert.AreEqual(plainText[i], pt[i], $"Failed on index {i}");
            }

            Assert.Pass();
        }
    }
}
