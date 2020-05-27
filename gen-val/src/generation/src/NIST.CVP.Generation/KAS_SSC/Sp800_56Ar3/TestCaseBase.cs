using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public abstract class TestCaseBase<TTestGroup, TTestCase, TKeyPair> : ITestCase<TTestGroup, TTestCase>
		where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TKeyPair : IDsaKeyPair
	{
		public int TestCaseId { get; set; }
		public TTestGroup ParentGroup { get; set; }
		public bool? TestPassed { get; }
		public bool Deferred { get; }
	}
}