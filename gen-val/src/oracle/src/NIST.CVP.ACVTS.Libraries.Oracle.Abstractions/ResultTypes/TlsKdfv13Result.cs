using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class TlsKdfv13Result
    {
        public TlsKdfV13FullResult DerivedKeyingMaterial { get; set; }

        public BitString Psk { get; set; }
        public BitString Dhe { get; set; }

        public BitString HelloClientRandom { get; set; }
        public BitString HelloServerRandom { get; set; }

        public BitString FinishClientRandom { get; set; }
        public BitString FinishServerRandom { get; set; }
    }
}
