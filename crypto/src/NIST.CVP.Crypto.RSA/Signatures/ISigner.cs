using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public interface ISigner
    {
        SignatureResult Sign(int nlen, BitString message, KeyPair key);
        VerifyResult Verify(int nlen, BitString signature, KeyPair key, BitString message);
    }
}
