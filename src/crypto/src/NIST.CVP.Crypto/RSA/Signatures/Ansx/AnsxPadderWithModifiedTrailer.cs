using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.Signatures.Ansx
{
    public class AnsxPadderWithModifiedTrailer : AnsxPadder
    {
        public AnsxPadderWithModifiedTrailer(ISha sha) : base(sha) { }

        public override PaddingResult Pad(int nlen, BitString message)
        {
            // 1. Message Hashing
            var hashedMessage = Sha.HashMessage(message).Digest;

            // 2. Hash Encapsulation
            var trailer = new BitString("66CC");    // ERROR: The first byte of the trailer is unexpected

            // Header is always 4, trailer is always 16
            var paddingLen = nlen - Header.BitLength - Sha.HashFunction.OutputLen - trailer.BitLength;
            var padding = GetPadding(paddingLen);

            var IR = Header.GetDeepCopy();
            IR = BitString.ConcatenateBits(IR, padding);
            IR = BitString.ConcatenateBits(IR, hashedMessage);
            IR = BitString.ConcatenateBits(IR, trailer);      // ERROR: Change the trailer to something else

            if (IR.BitLength != nlen)
            {
                return new PaddingResult("Improper length for IR");
            }

            return new PaddingResult(IR);
        }
    }
}
