using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class TestCase : ITestCase<TestGroup, TestCase>
	{
		public int TestCaseId { get; set; }
		public TestGroup ParentGroup { get; set; }
		public bool? TestPassed { get; set; }
		public bool Deferred => false;
		
		public KdfParameterOneStep KdfParameter { get; set; }
		public PartyFixedInfo FixedInfoPartyU { get; set; }
		public PartyFixedInfo FixedInfoPartyV { get; set; }
		public BitString Dkm { get; set; }
	}
}