using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class SshKdfResult
    {
        public BitString K { get; set; }
        public BitString H { get; set; }
        public BitString SessionId { get; set; }

        public BitString InitialIvClient { get; set; }
        public BitString EncryptionKeyClient { get; set; }
        public BitString IntegrityKeyClient { get; set; }

        public BitString InitialIvServer { get; set; }
        public BitString EncryptionKeyServer { get; set; }
        public BitString IntegrityKeyServer { get; set; }
    }
}
