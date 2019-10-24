using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface IMaskFactory
    {
        IMaskFunction GetMaskInstance(PssMaskTypes maskType, HashFunction hashFunction);
    }
}