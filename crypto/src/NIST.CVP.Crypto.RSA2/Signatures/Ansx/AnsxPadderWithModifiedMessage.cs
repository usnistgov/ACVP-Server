using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures.Ansx
{
    public class AnsxPadderWithModifiedMessage : AnsxPadder
    {
        public AnsxPadderWithModifiedMessage(ISha sha) : base(sha) { }

        public new (PublicKey key, BitString message, int nlen) PrePadCheck(PublicKey key, BitString message, int nlen)
        {
            return (key, message.BitStringAddition(BitString.Two()), nlen);
        }
    }
}
