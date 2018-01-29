using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class SignatureBuilder<TPrivateKey>
        where TPrivateKey : IRsaPrivateKey
    {
        private BitString _message;
        private PublicKey _publicKey;
        private TPrivateKey _privateKey;
        private IPaddingScheme _paddingScheme;
        private IRsa<TPrivateKey> _rsa;

        public SignatureBuilder<TPrivateKey> WithMessage(BitString message)
        {
            _message = message;
            return this;
        }

        public SignatureBuilder<TPrivateKey> WithPrivateKey(TPrivateKey privKey)
        {
            _privateKey = privKey;
            return this;
        }

        public SignatureBuilder<TPrivateKey> WithPublicKey(PublicKey pubKey)
        {
            _publicKey = pubKey;
            return this;
        }

        public SignatureBuilder<TPrivateKey> WithPaddingScheme(IPaddingScheme paddingScheme)
        {
            _paddingScheme = paddingScheme;
            return this;
        }

        public SignatureBuilder<TPrivateKey> WithDecryptionScheme(IRsa<TPrivateKey> rsa)
        {
            _rsa = rsa;
            return this;
        }

        public SignatureResult Build()
        {
            if (_paddingScheme == null || _rsa == null || _publicKey == null || _privateKey == null)
            {
                return new SignatureResult("Improper signature build");
            }

            var paddedMessage = _paddingScheme.Pad(_publicKey.N.ExactBitLength(), _message);
            if (!paddedMessage.Success)
            {
                return new SignatureResult(paddedMessage.ErrorMessage);
            }

            var signature = _rsa.Decrypt(paddedMessage.PaddedMessage.ToPositiveBigInteger(), _publicKey, _privateKey);
            var postCheck = _paddingScheme.PostSignCheck(signature, _publicKey);

            return new SignatureResult(postCheck);
        }
    }
}
