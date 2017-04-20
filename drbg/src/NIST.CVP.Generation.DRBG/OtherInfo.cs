using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG
{
    public class OtherInfo
    {
        public BitString AdditionalInput { get; set; }
        public BitString EntropyInput { get; set; }
    }
}
