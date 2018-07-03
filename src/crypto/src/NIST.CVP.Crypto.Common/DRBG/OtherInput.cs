using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.DRBG
{
    public class OtherInput
    {
        public BitString EntropyInput { get; set; }
        public BitString AdditionalInput { get; set; }
    }
}
