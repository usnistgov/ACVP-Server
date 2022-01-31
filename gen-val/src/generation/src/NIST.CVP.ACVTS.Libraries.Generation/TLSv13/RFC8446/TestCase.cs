using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.TLSv13.RFC8446
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => false;

        public BitString Psk { get; set; }
        public BitString Dhe { get; set; }

        public BitString HelloClientRandom { get; set; }
        public BitString HelloServerRandom { get; set; }

        public BitString FinishedClientRandom { get; set; }
        public BitString FinishedServerRandom { get; set; }

        public BitString ClientEarlyTrafficSecret { get; set; }
        public BitString EarlyExporterMasterSecret { get; set; }

        public BitString ClientHandshakeTrafficSecret { get; set; }
        public BitString ServerHandshakeTrafficSecret { get; set; }

        public BitString ClientApplicationTrafficSecret { get; set; }
        public BitString ServerApplicationTrafficSecret { get; set; }
        public BitString ExporterMasterSecret { get; set; }
        public BitString ResumptionMasterSecret { get; set; }
    }
}
