using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

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

        public SignatureResult Sign(int nlen, BigInteger message, PublicKey pubKey, IRsaPrivateKey privKey)
        {
            // Pad
            var paddedMessage = _paddingScheme.Pad(nlen, new BitString(message)).PaddedMessage;

            // Decrypt
            var signature = _rsa.Decrypt(paddedMessage.ToPositiveBigInteger(), pubKey, privKey);

            // Post-sign check
            var checkedSignature = _paddingScheme.PostSignCheck(signature, pubKey);

            return new SignatureResult(checkedSignature);
        }

        public VerifyResult Verify(int nlen, BigInteger message, BigInteger signature, PublicKey pubKey)
        {
            // Encrypt
            var embeddedMessage = _rsa.Encrypt(signature, pubKey);

            // Verify padding
            //var paddingResult = _paddingScheme.VerifyPadding(nlen, signature, message, pubKey);
            
            throw new NotImplementedException();
        }
    }
}
