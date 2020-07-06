using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF.TwoStep
{
	public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
	{
		public int VectorSetId { get; set; }
		public string Algorithm { get; set; }
		public string Mode { get; set; }
		public string Revision { get; set; }
		public bool IsSample { get; set; }
		public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
	}
}