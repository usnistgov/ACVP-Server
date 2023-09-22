using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface IMaskFactory
    {
        IMaskFunction GetMaskInstance(PssMaskTypes maskType, HashFunction hashFunction, int outputLen);
    }
}
