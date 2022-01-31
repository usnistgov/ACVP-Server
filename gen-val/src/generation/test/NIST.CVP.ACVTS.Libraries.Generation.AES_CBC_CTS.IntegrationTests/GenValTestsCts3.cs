using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.IntegrationTests
{
    public class GenValTestsCts3 : GenValTestsCtsBase
    {
        public override string Algorithm { get; } = "ACVP-AES-CBC-CS3";
        public override AlgoMode AlgoMode => AlgoMode.AES_CBC_CS3_v1_0;
    }
}
