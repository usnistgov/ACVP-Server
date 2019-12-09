using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.Signatures.Ansx
{
    public class AnsxPadderWithModifiedSignature : AnsxPadder
    {
        public AnsxPadderWithModifiedSignature(ISha sha) : base(sha) { }

        public override BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return BigInteger.Min(signature, pubKey.N - signature) + 2;
        }
    }
}
