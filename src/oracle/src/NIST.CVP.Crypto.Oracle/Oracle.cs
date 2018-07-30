using NIST.CVP.Common.Oracle;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private readonly Random800_90 _rand = new Random800_90();

        private const double GCM_FAIL_RATIO = .25;
        private const double XPN_FAIL_RATIO = .25;
        private const double CMAC_FAIL_RATIO = .25;
        private const double KEYWRAP_FAIL_RATIO = .2;
        private const double KMAC_FAIL_RATIO = .5;

        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;
    }
}
