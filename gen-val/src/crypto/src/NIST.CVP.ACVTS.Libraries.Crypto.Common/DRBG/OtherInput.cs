using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG
{
    public class OtherInput
    {
        public string IntendedUse { get; set; }
        public BitString EntropyInput { get; set; }
        public BitString AdditionalInput { get; set; }
    }
}
