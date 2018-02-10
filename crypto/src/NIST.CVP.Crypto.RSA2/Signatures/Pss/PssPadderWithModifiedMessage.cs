using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.Signatures.Pss
{
    public class PssPadderWithModifiedMessage : PssPadder
    {
        public PssPadderWithModifiedMessage(ISha sha, IEntropyProvider entropy, int saltLength) : base(sha, entropy, saltLength) { }

        public new (PublicKey key, BitString message, int nlen) PrePadCheck(PublicKey key, BitString message, int nlen)
        {
            return (key, message.BitStringAddition(BitString.Two()), nlen);
        }
    }
}
