using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures.Pkcs
{
    public class PkcsPadderWithModifiedTrailer : PkcsPadder
    {
        public PkcsPadderWithModifiedTrailer(ISha sha) : base(sha) { }

        public override PaddingResult Pad(int nlen, BitString message)
        {
            if (message.BitLength < GetHashAlgId().BitLength + 11 * 8)
            {
                return new PaddingResult("Message length too short");
            }

            var H = Sha.HashMessage(message).Digest;
            var T = BitString.ConcatenateBits(GetHashAlgId(), H);

            var psLen = nlen - (GetHashAlgId().BitLength + Sha.HashFunction.OutputLen) - 24;
            var PS = BitString.Ones(psLen);

            var EM = new BitString("00");
            EM = BitString.ConcatenateBits(EM, new BitString("01"));
            EM = BitString.ConcatenateBits(EM, PS);
            EM = BitString.ConcatenateBits(EM, new BitString("44"));        // ERROR: Should be 00
            EM = BitString.ConcatenateBits(EM, T);

            return new PaddingResult(EM);
        }
    }
}
