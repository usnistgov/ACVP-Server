using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;

namespace NIST.CVP.Crypto.RSA.Signatures.Pss
{
    public class PssPadderWithModifiedSignature : PssPadder
    {
        public PssPadderWithModifiedSignature(ISha sha, IMaskFunction mask, IEntropyProvider entropy, int saltLength) : base(sha, mask, entropy, saltLength) { }

        public override BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return signature + 2;
        }
    }
}
