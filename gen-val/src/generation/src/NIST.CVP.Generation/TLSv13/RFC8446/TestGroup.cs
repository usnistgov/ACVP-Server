using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TLSv13.RFC8446
{
	public class TestGroup : ITestGroup<TestGroup, TestCase>
	{
		public int TestGroupId { get; set; }
		public string TestType { get; set; }
		public List<TestCase> Tests { get; set; } = new List<TestCase>();
		public HashFunctions HashAlg { get; set; }
		public int RandomLength { get; set; }
	}
}