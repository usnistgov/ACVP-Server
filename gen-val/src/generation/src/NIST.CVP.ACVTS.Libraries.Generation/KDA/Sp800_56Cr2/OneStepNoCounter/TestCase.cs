using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStepNoCounter
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred => false;

        public KdfParameterOneStepNoCounter KdfParameter { get; set; }
        public PartyFixedInfo FixedInfoPartyU { get; set; }
        public PartyFixedInfo FixedInfoPartyV { get; set; }
        public BitString Dkm { get; set; }
    }
}
