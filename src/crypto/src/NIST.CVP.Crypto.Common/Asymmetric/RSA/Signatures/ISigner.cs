using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface ISigner
    {
        SignatureResult Sign(int nlen, BitString message, IKeyPair key);
        VerifyResult Verify(int nlen, BitString signature, IKeyPair key, BitString message);
    }
}
