using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures.Ansx
{
    public class AnsxPadderWithModifiedSignature : AnsxPadder
    {
        public AnsxPadderWithModifiedSignature(ISha sha) : base(sha) { }

        public new BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return BigInteger.Min(signature, pubKey.N - signature) + 2;
        }
    }
}
