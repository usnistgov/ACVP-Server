using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class SignerVerifier : ISignerVerifier<IRsaPrivateKey>
    {
        private readonly IPaddingScheme _paddingScheme;
        private readonly IRsa<IRsaPrivateKey> _rsa;

        public SignerVerifier(IRsa<IRsaPrivateKey> rsa, IPaddingScheme padding)
        {
            _rsa = rsa;
            _paddingScheme = padding;
        }

        public SignatureResult Sign(BigInteger message, PublicKey pubKey, IRsaPrivateKey privKey)
        {
            throw new NotImplementedException();
        }

        public VerifyResult Verify(BigInteger message, BigInteger signature, PublicKey pubKey, IRsaPrivateKey privKey)
        {
            throw new NotImplementedException();
        }
    }
}
