using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public interface ISignerVerifier<in TPrivateKey>
        where TPrivateKey : IRsaPrivateKey
    {
        SignatureResult Sign(BigInteger message, PublicKey pubKey, TPrivateKey privKey);
        VerifyResult Verify(BigInteger message, BigInteger signature, PublicKey pubKey, TPrivateKey privKey);
    }
}
