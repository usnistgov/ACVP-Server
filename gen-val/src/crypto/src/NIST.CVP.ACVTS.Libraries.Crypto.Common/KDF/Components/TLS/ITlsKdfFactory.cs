using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS
{
    public interface ITlsKdfFactory
    {
        ITlsKdf GetTlsKdfInstance(TlsModes tlsMode, HashFunction hash);
    }
}
