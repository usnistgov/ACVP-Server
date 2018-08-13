using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface IPaddingFactory
    {
        IPaddingScheme GetPaddingScheme(SignatureSchemes sigMode, ISha sha, IEntropyProvider entropyProvider = null, int saltLength = 0);
        IPaddingScheme GetSigningPaddingScheme(SignatureSchemes sigMode, ISha sha, SignatureModifications errors, IEntropyProvider entropyProvider = null, int saltLength = 0);
    }
}
