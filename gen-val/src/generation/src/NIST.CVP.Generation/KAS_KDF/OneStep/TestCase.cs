using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class TestCase : ITestCase<TestGroup, TestCase>
	{
		public int TestCaseId { get; set; }
		public TestGroup ParentGroup { get; set; }
		public bool? TestPassed { get; }
		public bool Deferred { get; }
		
		public KdfParameterOneStep KdfParameter { get; set; }
	}
}