using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class SignatureBuilder
    {
        private BitString _message;
        private PublicKey _publicKey;
        private PrivateKeyBase _privateKey;
        private IPaddingScheme _paddingScheme;
        private IRsa _rsa;

        public SignatureBuilder WithMessage(BitString message)
        {
            _message = message;
            return this;
        }

        public SignatureBuilder WithPrivateKey(PrivateKeyBase privKey)
        {
            _privateKey = privKey;
            return this;
        }

        public SignatureBuilder WithPublicKey(PublicKey pubKey)
        {
            _publicKey = pubKey;
            return this;
        }

        public SignatureBuilder WithKey(KeyPair key)
        {
            _publicKey = key.PubKey;
            _privateKey = key.PrivKey;
            return this;
        }

        public SignatureBuilder WithPaddingScheme(IPaddingScheme paddingScheme)
        {
            _paddingScheme = paddingScheme;
            return this;
        }

        public SignatureBuilder WithDecryptionScheme(IRsa rsa)
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

            var signature = _rsa.Decrypt(paddedMessage.PaddedMessage.ToPositiveBigInteger(), _privateKey, _publicKey);
            var postCheck = _paddingScheme.PostSignCheck(signature, _publicKey);

            return new SignatureResult(postCheck);
        }
    }
}
