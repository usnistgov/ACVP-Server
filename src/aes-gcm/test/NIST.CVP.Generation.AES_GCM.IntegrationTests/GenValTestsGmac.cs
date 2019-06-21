using NIST.CVP.Common;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
{
    public class GenValTestsGmac : GenValTestsBase
    {
        public override AlgoMode AlgoMode => AlgoMode.AES_GMAC_v1_0;
        public override string Algorithm => "ACVP-AES-GMAC";
    }
}