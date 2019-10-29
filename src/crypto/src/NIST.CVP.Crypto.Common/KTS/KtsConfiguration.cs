using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KTS
{
    public class KtsConfiguration
    {
        public KasHashAlg HashAlg { get; set; }
        public string AssociatedDataPattern { get; set; }
        public FixedInfoEncoding Encoding { get; set; }
    }
}