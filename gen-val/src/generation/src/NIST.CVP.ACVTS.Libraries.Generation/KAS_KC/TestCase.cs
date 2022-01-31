using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; }
        public bool Deferred { get; }
        public PartyFixedInfo MacDataServer { get; set; }
        public PartyFixedInfo MacDataIut { get; set; }
        public BitString MacKey { get; set; }
        public BitString MacData { get; set; }
        public BitString Tag { get; set; }
        public KasKcDisposition Disposition { get; set; }
    }
}
