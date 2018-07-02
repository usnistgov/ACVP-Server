using System.Numerics;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class RsaKeyResult
    {
        // Lots of inputs go here
        public BigInteger E { get; set; }

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger N { get; set; }
        public BigInteger D { get; set; }
    }
}
