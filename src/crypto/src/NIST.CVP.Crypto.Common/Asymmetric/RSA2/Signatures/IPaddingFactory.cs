using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures
{
    public interface IPaddingFactory
    {
        IPaddingScheme GetPaddingScheme(SignatureSchemes sigMode, ISha sha, IEntropyProvider entropyProvider = null, int saltLength = 0);
        IPaddingScheme GetSigningPaddingScheme(SignatureSchemes sigMode, ISha sha, SignatureModifications errors, IEntropyProvider entropyProvider = null, int saltLength = 0);
    }
}
