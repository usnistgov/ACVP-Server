using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Pkcs
{
    public class PkcsPadderWithModifiedMessage : PkcsPadder
    {
        public PkcsPadderWithModifiedMessage(ISha sha) : base(sha) { }

        public override (KeyPair key, BitString message, int nlen) PrePadCheck(KeyPair key, BitString message, int nlen)
        {
            return (key, BitString.BitStringAddition(message, BitString.Two()), nlen);
        }
    }
}
