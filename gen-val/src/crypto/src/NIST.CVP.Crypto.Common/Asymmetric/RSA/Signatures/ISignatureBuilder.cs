﻿using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface ISignatureBuilder
    {
        SignatureResult BuildSign();
        VerifyResult BuildVerify();
        ISignatureBuilder WithDecryptionScheme(IRsa rsa);
        ISignatureBuilder WithKey(KeyPair key);
        ISignatureBuilder WithMessage(BitString message);
        ISignatureBuilder WithPaddingScheme(IPaddingScheme paddingScheme);
        ISignatureBuilder WithPrivateKey(PrivateKeyBase privKey);
        ISignatureBuilder WithPublicKey(PublicKey pubKey);
        ISignatureBuilder WithSignature(BitString signature);
    }
}