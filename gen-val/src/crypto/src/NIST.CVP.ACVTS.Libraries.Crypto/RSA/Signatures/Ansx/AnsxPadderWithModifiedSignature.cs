using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Ansx
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
