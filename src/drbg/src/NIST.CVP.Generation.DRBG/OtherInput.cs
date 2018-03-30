using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG
{
    public class OtherInput
    {
        public BitString AdditionalInput { get; set; }
        public BitString EntropyInput { get; set; }
    }
}
