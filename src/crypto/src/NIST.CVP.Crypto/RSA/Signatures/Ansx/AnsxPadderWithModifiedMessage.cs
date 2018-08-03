using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.Signatures.Ansx
{
    public class AnsxPadderWithModifiedMessage : AnsxPadder
    {
        public AnsxPadderWithModifiedMessage(ISha sha) : base(sha) { }

        public override (KeyPair key, BitString message, int nlen) PrePadCheck(KeyPair key, BitString message, int nlen)
        {
            return (key, message.BitStringAddition(BitString.Two()), nlen);
        }
    }
}
