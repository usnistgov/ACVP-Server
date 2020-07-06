using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF.TwoStep
{
	public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
	{
		public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
		{
			var list =
				new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>()
				{
					new TestGroupGenerator(),
				};

			return list;
		}
	}
}