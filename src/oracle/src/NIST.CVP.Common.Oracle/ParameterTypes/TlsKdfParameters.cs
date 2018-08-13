using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class TlsKdfParameters
    {
        public TlsModes TlsMode { get; set; }
        public HashFunction HashAlg { get; set; }
        public int PreMasterSecretLength { get; set; }
        public int KeyBlockLength { get; set; }
    }
}
