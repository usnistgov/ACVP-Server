using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class TlsKdfParameters
    {
        public TlsModes TlsMode { get; set; }
        public HashFunction HashAlg { get; set; }
        public int PreMasterSecretLength { get; set; }
        public int KeyBlockLength { get; set; }
    }
}
