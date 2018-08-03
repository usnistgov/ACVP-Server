using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class TlsKdfResult
    {
        public BitString PreMasterSecret { get; set; }
        public BitString ClientHelloRandom { get; set; }
        public BitString ServerHelloRandom { get; set; }
        public BitString ClientRandom { get; set; }
        public BitString ServerRandom { get; set; }

        public BitString MasterSecret { get; set; }
        public BitString KeyBlock { get; set; }
    }
}
