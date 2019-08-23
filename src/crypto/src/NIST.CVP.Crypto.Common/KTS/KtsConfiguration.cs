using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KTS
{
    public class KtsConfiguration
    {
        public KasHashAlg KtsHashAlg { get; set; }
        public string AssociatedDataPattern { get; set; }
    }
}