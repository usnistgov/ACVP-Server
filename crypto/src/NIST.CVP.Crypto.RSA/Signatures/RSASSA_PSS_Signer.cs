using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class RSASSA_PSS_Signer : SignerBase
    {
        public RSASSA_PSS_Signer(HashFunction hashFunction) : base(hashFunction) { }

        public override SignatureResult Sign(int nlen, BitString message, KeyPair key)
        {
            throw new NotImplementedException();
        }

        public override VerifyResult Verify(int nlen, BitString signature, KeyPair key, BitString message)
        {
            throw new NotImplementedException();
        }
    }
}
