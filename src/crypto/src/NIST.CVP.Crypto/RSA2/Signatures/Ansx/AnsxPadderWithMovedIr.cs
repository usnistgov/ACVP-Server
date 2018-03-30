using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures.Ansx
{
    public class AnsxPadderWithMovedIr : AnsxPadder
    {
        public AnsxPadderWithMovedIr(ISha sha) : base(sha) { }

        public override PaddingResult Pad(int nlen, BitString message)
        {
            // 1. Message Hashing
            var hashedMessage = Sha.HashMessage(message).Digest;

            // 2. Hash Encapsulation
            var trailer = GetTrailer();

            // Header is always 4, trailer is always 16
            var paddingLen = nlen - Header.BitLength - Sha.HashFunction.OutputLen - trailer.BitLength;
            var padding = GetPadding(paddingLen);

            // ERROR: Split the padding into two chunks and put the hashed message in the middle
            var firstChunkPadding = padding.GetMostSignificantBits(paddingLen - 8);
            var secondChunkPadding = padding.GetLeastSignificantBits(8);

            var IR = Header.GetDeepCopy();
            IR = BitString.ConcatenateBits(IR, firstChunkPadding);
            IR = BitString.ConcatenateBits(IR, hashedMessage);
            IR = BitString.ConcatenateBits(IR, secondChunkPadding);
            IR = BitString.ConcatenateBits(IR, trailer);

            if(IR.BitLength != nlen)
            {
                return new PaddingResult("Improper length for IR");
            }

            return new PaddingResult(IR);
        }
    }
}
