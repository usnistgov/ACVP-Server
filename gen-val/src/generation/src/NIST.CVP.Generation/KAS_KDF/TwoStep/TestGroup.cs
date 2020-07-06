using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF.TwoStep
{
	public class TestGroup : ITestGroup<TestGroup, TestCase>
	{
		public int TestGroupId { get; set; }
		public string TestType { get; set; }
		public List<TestCase> Tests { get; set; } = new List<TestCase>();
		public bool IsSample { get; set; }
		public TwoStepConfiguration KdfConfiguration { get; set; }
		public int ZLength { get; set; }
	}
}