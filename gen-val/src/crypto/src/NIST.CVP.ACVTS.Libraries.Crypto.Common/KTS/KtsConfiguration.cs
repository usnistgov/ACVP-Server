using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS
{
    public class KtsConfiguration
    {
        public KasHashAlg HashAlg { get; set; }
        public string AssociatedDataPattern { get; set; }
        public FixedInfoEncoding Encoding { get; set; }
    }
}
