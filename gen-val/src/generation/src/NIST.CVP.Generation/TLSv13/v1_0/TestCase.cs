using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TLSv13.v1_0
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

		public BitString ExporterMasterSecret { get; set; }
	}
}