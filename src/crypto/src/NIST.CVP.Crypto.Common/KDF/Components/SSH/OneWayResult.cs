using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.SSH
{
    public class OneWayResult
    {
        public BitString InitialIv { get; set; }
        public BitString EncryptionKey { get; set; }
        public BitString IntegrityKey { get; set; }
    }
}
