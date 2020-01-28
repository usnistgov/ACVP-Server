using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.Signatures.Pkcs
{
    public class PkcsPadderWithModifiedSignature : PkcsPadder
    {
        public PkcsPadderWithModifiedSignature(ISha sha) : base(sha) { }

        public override BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return signature + 2;
        }
    }
}
