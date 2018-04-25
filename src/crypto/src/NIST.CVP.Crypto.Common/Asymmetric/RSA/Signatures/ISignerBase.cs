using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface ISignerBase
    {
        void AddEntropy(BitString entropy);
        SignatureResult ModifyIRTrailerSign(int nlen, BitString message, IKeyPair key);
        SignatureResult MoveIRSign(int nlen, BitString message, IKeyPair key);
        void SetHashFunction(HashFunction hf);
        void SetSaltLen(int saltLen);
        SignatureResult Sign(int nlen, BitString message, IKeyPair key);
        SignatureResult SignWithErrors(int nlen, BitString message, IKeyPair key, FailureReasons reason);
        VerifyResult Verify(int nlen, BitString signature, IKeyPair key, BitString message);
    }
}