using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public abstract class TestVectorSetBase<TTestGroup, TTestCase, TKeyPair> : ITestVectorSet<TTestGroup, TTestCase>
		where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TKeyPair : IDsaKeyPair
	{
		public int VectorSetId { get; set; }
		public string Algorithm { get; set; }
		public string Mode { get; set; }
		public string Revision { get; set; }
		public bool IsSample { get; set; }
		public List<TTestGroup> TestGroups { get; set; } = new List<TTestGroup>();
	}
}