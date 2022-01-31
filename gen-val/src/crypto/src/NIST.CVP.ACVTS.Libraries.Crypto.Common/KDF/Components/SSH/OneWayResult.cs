using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH
{
    public class OneWayResult
    {
        public BitString InitialIv { get; set; }
        public BitString EncryptionKey { get; set; }
        public BitString IntegrityKey { get; set; }
    }
}
