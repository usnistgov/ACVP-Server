using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.IntegrationTests
{
    public class GenValTestsGcm : GenValTestsBase
    {
        public override AlgoMode AlgoMode => AlgoMode.AES_GCM_v1_0;
        public override string Algorithm => "ACVP-AES-GCM";
    }
}
