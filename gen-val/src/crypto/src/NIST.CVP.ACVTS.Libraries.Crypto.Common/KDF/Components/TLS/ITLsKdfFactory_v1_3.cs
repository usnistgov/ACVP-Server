using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS
{
    public interface ITLsKdfFactory_v1_3
    {
        ITlsKdf_v1_3 GetInstance(HashFunctions hashFunction);
    }
}
