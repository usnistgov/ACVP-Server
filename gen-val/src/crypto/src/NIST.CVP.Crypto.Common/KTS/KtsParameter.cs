using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KTS
{
    public class KtsParameter
    {
        public KasHashAlg KtsHashAlg { get; set; }
        public string AssociatedDataPattern { get; set; }
        public FixedInfoEncoding Encoding { get; set; }
    }
}