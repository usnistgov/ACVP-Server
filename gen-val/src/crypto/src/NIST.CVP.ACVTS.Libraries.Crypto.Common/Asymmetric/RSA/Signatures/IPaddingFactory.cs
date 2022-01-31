using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface IPaddingFactory
    {
        IPaddingScheme GetPaddingScheme(SignatureSchemes sigMode, ISha sha, PssMaskTypes maskType = PssMaskTypes.None, IEntropyProvider entropyProvider = null, int saltLength = 0);
        IPaddingScheme GetSigningPaddingScheme(SignatureSchemes sigMode, ISha sha, SignatureModifications errors, PssMaskTypes maskType = PssMaskTypes.None, IEntropyProvider entropyProvider = null, int saltLength = 0);
    }
}
