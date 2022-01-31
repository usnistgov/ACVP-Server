using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH
{
    public interface ISshFactory
    {
        ISsh GetSshInstance(HashFunction hash, Cipher cipher);
    }
}
