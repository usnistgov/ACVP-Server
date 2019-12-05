using NIST.CVP.Common;

namespace NIST.CVP.Generation.AES_CBC_CTS.IntegrationTests
{
    public class GenValTestsCts1 : GenValTestsCtsBase
    {
        public override string Algorithm { get; } = "ACVP-AES-CBC-CS1";
        public override AlgoMode AlgoMode => AlgoMode.AES_CBC_CS1_v1_0;
    }
}
