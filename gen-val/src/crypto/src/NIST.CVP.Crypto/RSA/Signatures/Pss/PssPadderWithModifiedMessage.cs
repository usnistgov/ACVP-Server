using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA.Signatures.Pss
{
    public class PssPadderWithModifiedMessage : PssPadder
    {
        public PssPadderWithModifiedMessage(ISha sha, IMaskFunction mask, IEntropyProvider entropy, int saltLength) : base(sha, mask, entropy, saltLength) { }

        public override (KeyPair key, BitString message, int nlen) PrePadCheck(KeyPair key, BitString message, int nlen)
        {
            return (key, message.BitStringAddition(BitString.Two()), nlen);
        }
    }
}
