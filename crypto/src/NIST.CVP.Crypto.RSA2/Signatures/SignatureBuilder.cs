using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class SignatureBuilder : ISignatureBuilder
    {
        private BitString _message;
        private BitString _signature;
        private PublicKey _publicKey;
        private PrivateKeyBase _privateKey;
        private IPaddingScheme _paddingScheme;
        private IRsa _rsa;

        public SignatureBuilder WithMessage(BitString message)
        {
            _message = message;
            return this;
        }

        public SignatureBuilder WithSignature(BitString signature)
        {
            _signature = signature;
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

        public VerifyResult BuildVerify()
        {
            // Check for empty values that prevent building
            if (_paddingScheme == null || _rsa == null || _publicKey == null || _message == null || _signature == null)
            {
                return new VerifyResult("Improper verify build");
            }

            // Encrypt with the public key to open the signature
            var embeddedMessage = _rsa.Encrypt(_signature.ToPositiveBigInteger(), _publicKey);

            // Verify the padding
            return _paddingScheme.VerifyPadding(_publicKey.N.ExactBitLength(), _message, embeddedMessage, _publicKey);
        }

        public SignatureResult BuildSign()
        {
            // Check for empty values that prevent building
            if (_paddingScheme == null || _rsa == null || _publicKey == null || _privateKey == null || _message == null)
            {
                return new SignatureResult("Improper signature build");
            }

            // Do provided padding method either correct or incorrect
            var paddedResult = _paddingScheme.Pad(_publicKey.N.ExactBitLength(), _message);
            if (!paddedResult.Success)
            {
                return new SignatureResult(paddedResult.ErrorMessage);
            }

            // Perform the RSA Decryption
            var signature = _rsa.Decrypt(paddedResult.PaddedMessage.ToPositiveBigInteger(), _privateKey, _publicKey);
            
            // Perform the Post-Check depending on the padding scheme
            var postCheck = _paddingScheme.PostSignCheck(signature, _publicKey);

            return new SignatureResult(postCheck);
        }
    }
}

