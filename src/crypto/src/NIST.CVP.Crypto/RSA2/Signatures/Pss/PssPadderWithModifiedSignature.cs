using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.Signatures.Pss
{
    public class PssPadderWithModifiedSignature : PssPadder
    {
        public PssPadderWithModifiedSignature(ISha sha, IEntropyProvider entropy, int saltLength) : base(sha, entropy, saltLength) { }

        public override BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return signature + 2;
        }
    }
}
