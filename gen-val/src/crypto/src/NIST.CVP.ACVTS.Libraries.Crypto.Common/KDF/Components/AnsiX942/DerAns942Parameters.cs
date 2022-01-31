using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942
{
    public class DerAns942Parameters : IAns942Parameters
    {
        public BitString Zz { get; set; }
        public int KeyLen { get; set; }
        public BitString Oid { get; set; }
        public BitString PartyUInfo { get; set; }
        public BitString PartyVInfo { get; set; }
        public BitString SuppPubInfo { get; set; }
        public BitString SuppPrivInfo { get; set; }
    }
}
