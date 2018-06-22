using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2.Signatures.Pkcs
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
