using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class SrtpKdfResult
    {
        public BitString MasterKey { get; set; }
        public BitString MasterSalt { get; set; }
        public BitString Index { get; set; }
        public BitString SrtcpIndex { get; set; }

        public BitString SrtpEncryptionKey { get; set; }
        public BitString SrtpAuthenticationKey { get; set; }
        public BitString SrtpSaltingKey { get; set; }
        public BitString SrtcpEncryptionKey { get; set; }
        public BitString SrtcpAuthenticationKey { get; set; }
        public BitString SrtcpSaltingKey { get; set; }
    }
}
