using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
	{
		public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
		{
			throw new NotImplementedException();

			var groups = new List<TestGroup>();

			foreach (var auxFunction in parameters.AuxFunctions)
			{
				foreach (var fixedInfoEncoding in parameters.FixedInfoEncoding)
				{
					
				}
			}
			
			return Task.FromResult(groups);
		}
	}
}