using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public interface ISignatureBuilder
    {
        SignatureResult BuildSign();
        VerifyResult BuildVerify();
        SignatureBuilder WithDecryptionScheme(IRsa rsa);
        SignatureBuilder WithKey(KeyPair key);
        SignatureBuilder WithMessage(BitString message);
        SignatureBuilder WithPaddingScheme(IPaddingScheme paddingScheme);
        SignatureBuilder WithPrivateKey(PrivateKeyBase privKey);
        SignatureBuilder WithPublicKey(PublicKey pubKey);
        SignatureBuilder WithSignature(BitString signature);
    }
}