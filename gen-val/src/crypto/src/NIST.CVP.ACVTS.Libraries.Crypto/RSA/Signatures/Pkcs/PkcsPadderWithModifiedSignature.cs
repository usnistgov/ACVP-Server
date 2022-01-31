using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Pkcs
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
