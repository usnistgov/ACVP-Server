using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ANSIX942
{
    public class AnsiX942Der : IAnsiX942
    {
        private readonly ISha _sha;

        public AnsiX942Der(ISha sha)
        {
            _sha = sha;
        }

        public KdfResult DeriveKey(IAns942Parameters param)
        {
            if (!(param is DerAns942Parameters derParams))
            {
                return new KdfResult("Unable to parse der parameters");
            }

            if (derParams.KeyLen <= 0 || derParams.KeyLen > 65536)
            {
                return new KdfResult($"KeyLen must be between [1, 65536]. Value given was: {derParams.KeyLen}");
            }

            var d = (int)System.Math.Ceiling(derParams.KeyLen / (decimal)_sha.HashFunction.OutputLen);
            var h = new BitString(0);
            var counter = BitString.To32BitString(0);

            for (var i = 1; i <= d; i++)
            {
                // Increment Counter
                counter = counter.BitStringAddition(BitString.One());

                // Prepare ANS.1/DER encoded OtherInfo
                var derEncodedOtherInfo = DerEncode(derParams, counter);
                var str = derEncodedOtherInfo.ToHex();
                // H[i] = Hash(ZZ || otherInfo)
                h = h.ConcatenateBits(_sha.HashMessage(derParams.Zz.ConcatenateBits(derEncodedOtherInfo)).Digest);
            }

            return new KdfResult(h.GetMostSignificantBits(derParams.KeyLen));
        }

        // 30 1d
        //   30 13
        //     06 0b 2a 86 48 86 f7 0d 01 09 10 03 06          ; 3DES wrap OID
        //     04 04
        //       00 00 00 01                                   ; Counter
        //                                                     ; a0 omitted (not present), PartyAInfo (Optional)
        //   a2 06                                             ; a2 is flag for supplemental info (keyLength)
        //     04 04
        //       00 00 00 c0                                   ; KeyLen (192 bits in this example)

        // 30 61
        //   30 13
        //     06 0b 2a 86 48 86 f7 0d 01 09 10 03 06
        //     04 04
        //       00 00 00 01
        //   a0 42
        //     04 40 ff e9 be b1 2d 30 aa a6 a6 2d 7f 6e eb 1c f3 bb 43 d4 44 53 4d 61 b4 4f f5 d6 67 5b f5 11 dd b5 d8 4a 55 24 64 fa 40 f8 3e cd 07 29 9c 18 04 b9 37 6d 4a c0 90 12 aa 0b 7a 51 a4 ea 96 ab ae f6 
        //   a2 06
        //     04 04
        //       00 00 00 c0

        // 30 1D
        //   30 13
        //     06 0B 2A 86 48 86 F7 0D 01 09 10 03 06
        //     04 04
        //       00 00 00 01
        //   A2 06
        //     04 04
        //       00 00 01 00

        private BitString DerEncode(DerAns942Parameters derParams, BitString counter)
        {
            // Octet type || 4 bytes || value
            var counterOctet = AnsDerEncodingHelper.EncodeOctet(counter);

            // Concatenate OID and Counter as a sequence
            var algorithmIdentifier = AnsDerEncodingHelper.EncodeSequence(derParams.Oid.ConcatenateBits(counterOctet));

            // All optional information
            // a0-a3 || len || value
            var wrappedOtherInfo = new BitString(0);
            if (derParams.PartyUInfo.BitLength > 0)
            {
                wrappedOtherInfo = AnsDerEncodingHelper.EncodePartyUInfo(derParams.PartyUInfo);
            }
            if (derParams.PartyVInfo.BitLength > 0)
            {
                wrappedOtherInfo = BitString.ConcatenateBits(wrappedOtherInfo, AnsDerEncodingHelper.EncodePartyVInfo(derParams.PartyVInfo));
            }
            if (derParams.SuppPubInfo.BitLength > 0)
            {
                wrappedOtherInfo = BitString.ConcatenateBits(wrappedOtherInfo, AnsDerEncodingHelper.EncodeSuppPubInfo(derParams.SuppPubInfo));
            }
            if (derParams.SuppPrivInfo.BitLength > 0)
            {
                wrappedOtherInfo = BitString.ConcatenateBits(wrappedOtherInfo, AnsDerEncodingHelper.EncodeSuppPrivInfo(derParams.SuppPrivInfo));
            }

            // OuterWrapper (Sequence) || length || content
            var fullEncoding = AnsDerEncodingHelper.EncodeSequence(algorithmIdentifier.ConcatenateBits(wrappedOtherInfo));

            return fullEncoding;
        }
    }
}
