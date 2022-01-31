using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC
{
    public class MacConfiguration
    {
        public KeyAgreementMacType MacType { get; set; }
        public int KeyLen { get; set; }
        public int MacLen { get; set; }
    }
}
