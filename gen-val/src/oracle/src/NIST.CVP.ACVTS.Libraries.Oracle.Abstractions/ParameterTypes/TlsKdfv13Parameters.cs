using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class TlsKdfv13Parameters
    {
        public HashFunctions HashAlg { get; set; }
        public TlsModes1_3 RunningMode { get; set; }
        public int RandomLength { get; set; }
    }
}
