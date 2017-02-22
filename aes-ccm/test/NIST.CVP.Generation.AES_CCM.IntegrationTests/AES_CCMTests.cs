using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.IntegrationTests
{
    [TestFixture]
    public class AES_CCMTests
    {

        AES_CCM _subject = new AES_CCM(new AES_CCMInternals(), new RijndaelFactory(new RijndaelInternals()));

        [Test]
        public void Test()
        {

            var NonceBytes = new byte[10];
            var AssocDataBytes = new byte[16];
            var PayloadBytes = new byte[16];
            var KeyBytes = new byte[16];

            KeyBytes[0] = 0xf3; KeyBytes[1] = 0x44;
            KeyBytes[2] = 0x81; KeyBytes[3] = 0xec;
            KeyBytes[4] = 0x3c; KeyBytes[5] = 0xc6;
            KeyBytes[6] = 0x27; KeyBytes[7] = 0xba;
            KeyBytes[8] = 0xcd; KeyBytes[9] = 0x5d;
            KeyBytes[10] = 0xc3; KeyBytes[11] = 0xfb;
            KeyBytes[12] = 0x08; KeyBytes[13] = 0xf2;
            KeyBytes[14] = 0x73; KeyBytes[15] = 0xe6;

            AssocDataBytes[0] = 0x01; AssocDataBytes[1] = 0x47;
            AssocDataBytes[2] = 0x30; AssocDataBytes[3] = 0xf8;
            AssocDataBytes[4] = 0x0a; AssocDataBytes[5] = 0xc6;
            AssocDataBytes[6] = 0x25; AssocDataBytes[7] = 0xfe;
            AssocDataBytes[8] = 0x84; AssocDataBytes[9] = 0xf0;
            AssocDataBytes[10] = 0x26; AssocDataBytes[11] = 0xc6;
            AssocDataBytes[12] = 0x0b; AssocDataBytes[13] = 0xfd;
            AssocDataBytes[14] = 0x54; AssocDataBytes[15] = 0x7d;

            PayloadBytes[0] = 0x1b; PayloadBytes[1] = 0x07;
            PayloadBytes[2] = 0x7a; PayloadBytes[3] = 0x6a;
            PayloadBytes[4] = 0xf4; PayloadBytes[5] = 0xb7;
            PayloadBytes[6] = 0xf9; PayloadBytes[7] = 0x82;
            PayloadBytes[8] = 0x29; PayloadBytes[9] = 0xde;
            PayloadBytes[10] = 0x78; PayloadBytes[11] = 0x6d;
            PayloadBytes[12] = 0x75; PayloadBytes[13] = 0x16;
            PayloadBytes[14] = 0xb6; PayloadBytes[15] = 0x39;

            NonceBytes[0] = 0x01; NonceBytes[1] = 0x02;
            NonceBytes[2] = 0x03; NonceBytes[3] = 0x04;
            NonceBytes[4] = 0x05; NonceBytes[5] = 0x06;
            NonceBytes[6] = 0x07; NonceBytes[7] = 0x08;
            NonceBytes[8] = 0x09; NonceBytes[9] = 0x0a;


            byte[] expectedCtBytes = new byte[32];
            expectedCtBytes[0] = 0x57;
            expectedCtBytes[1] = 0x49;
            expectedCtBytes[2] = 0x5e;
            expectedCtBytes[3] = 0x8b;
            expectedCtBytes[4] = 0x5a;
            expectedCtBytes[5] = 0xe6;
            expectedCtBytes[6] = 0x28;
            expectedCtBytes[7] = 0xf9;
            expectedCtBytes[8] = 0x14;
            expectedCtBytes[9] = 0xf4;
            expectedCtBytes[10] = 0x78;
            expectedCtBytes[11] = 0xad;
            expectedCtBytes[12] = 0x32;
            expectedCtBytes[13] = 0x15;
            expectedCtBytes[14] = 0xea;
            expectedCtBytes[15] = 0x96;
            expectedCtBytes[16] = 0x1a;
            expectedCtBytes[17] = 0xb7;
            expectedCtBytes[18] = 0xcf;
            expectedCtBytes[19] = 0xeb;
            expectedCtBytes[20] = 0xa1;
            expectedCtBytes[21] = 0x18;
            expectedCtBytes[22] = 0x9a;
            expectedCtBytes[23] = 0x9c;
            expectedCtBytes[24] = 0xe7;
            expectedCtBytes[25] = 0x88;
            expectedCtBytes[26] = 0xe3;
            expectedCtBytes[27] = 0xd7;
            expectedCtBytes[28] = 0x86;
            expectedCtBytes[29] = 0x82;
            expectedCtBytes[30] = 0x21;
            expectedCtBytes[31] = 0x35;

            var ct = new BitString(expectedCtBytes);

            var key = new BitString(KeyBytes);
            var assoc = new BitString(AssocDataBytes);
            var payload = new BitString(PayloadBytes);
            var nonce = new BitString(NonceBytes);

            var result = _subject.Encrypt(key, nonce, payload, assoc, 128);
            
            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(ct, result.CipherText, nameof(ct));
        }
    }
}
