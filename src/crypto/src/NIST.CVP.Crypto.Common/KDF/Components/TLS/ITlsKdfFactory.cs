using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;

namespace NIST.CVP.Crypto.Common.KDF.Components.TLS
{
    public interface ITlsKdfFactory
    {
        ITlsKdf GetTlsKdfInstance(TlsModes tlsMode, HashFunction hash);
    }
}
