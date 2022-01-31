using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class TlsKdfResult
    {
        public BitString PreMasterSecret { get; set; }
        public BitString ClientHelloRandom { get; set; }
        public BitString ServerHelloRandom { get; set; }
        public BitString ClientRandom { get; set; }
        public BitString ServerRandom { get; set; }

        public BitString SessionHash { get; set; }

        public BitString MasterSecret { get; set; }
        public BitString KeyBlock { get; set; }
    }
}
