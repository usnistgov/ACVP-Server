using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.MAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.MD5
{
    public class HmacMd5 : IHmac
    {
        private readonly IMd5 _hash;

        // TODO find that method for repeating bitstring from bytes
        private readonly BitString _opad = new BitString("5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c");
        private readonly BitString _ipad = new BitString("36363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636");

        public int OutputLength => 128;

        public HmacMd5(IMd5 hash)
        {
            _hash = hash;
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            // From MD5
            var blockSize = 512;

            BitString shortenedKey;
            if (key.BitLength > blockSize)
            {
                shortenedKey = _hash.Hash(key).Digest;
            }
            else if (key.BitLength == blockSize)
            {
                shortenedKey = key.GetDeepCopy();
            }
            else
            {
                shortenedKey = key.ConcatenateBits(BitString.Zeroes(blockSize - key.BitLength));
            }

            var oKeyPad = _opad.XOR(shortenedKey);
            var iKeyPad = _ipad.XOR(shortenedKey);

            // HMAC_MD5 = Hash(oKeyPad || Hash(iKeyPad || message))
            var iKeyPadConcatMessage = iKeyPad.GetDeepCopy();
            if (message.BitLength != 0)
            {
                iKeyPadConcatMessage = iKeyPadConcatMessage.ConcatenateBits(message);
            }

            var innerDigest = _hash.Hash(iKeyPadConcatMessage).Digest;

            var oKeyPadConcatInnerDigest = oKeyPad.ConcatenateBits(innerDigest);
            var outerDigest = _hash.Hash(oKeyPadConcatInnerDigest).Digest;

            return new MacResult(outerDigest);
        }
    }
}
