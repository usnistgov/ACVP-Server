using NIST.CVP.Common.Oracle;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private readonly Random800_90 _rand = new Random800_90();

        private const double GCM_FAIL_RATIO = .25;
        private const double XPN_FAIL_RATIO = .25;
    }
}
