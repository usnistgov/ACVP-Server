using NIST.CVP.Common;

namespace NIST.CVP.Generation.AES_CBC_CTS.IntegrationTests
{
    public class GenValTestsCts3 : GenValTestsCtsBase
    {
        public override string Algorithm { get; } = "ACVP-AES-CBC-CS3";
        public override AlgoMode AlgoMode => AlgoMode.AES_CBC_CS3_v1_0;
    }
}
