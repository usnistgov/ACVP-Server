﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.MD5;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.MD5
{
    public class HmacMd5 : IHmac
    {
        private readonly IMd5 _hash;

        private readonly BitString _opad = new BitString(new byte[] { 0x5C }, 512);
        private readonly BitString _ipad = new BitString(new byte[] { 0x36 }, 512);

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

        public void Init(byte[] key)
        {
            throw new System.NotImplementedException();
        }

        public void FastInit()
        {
            throw new System.NotImplementedException();
        }

        public void Update(byte[] message, int bitLength)
        {
            throw new System.NotImplementedException();
        }

        public void Final(ref byte[] output, int macLen = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
