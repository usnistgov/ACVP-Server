using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums;

namespace NIST.CVP.Crypto.Common.KDF.Components.SSH
{
    public interface ISshFactory
    {
        ISsh GetSshInstance(HashFunction hash, Cipher cipher);
    }
}