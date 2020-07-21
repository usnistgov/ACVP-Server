using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.TLSv13.RFC8446
{
	public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
	{
		private readonly IRandom800_90 _random;

		public TestGroupGenerator(IRandom800_90 random)
		{
			_random = random;
		}
		
		public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
		{
			var testGroups = new List<TestGroup>();

			foreach (var hashAlg in parameters.HmacAlg)
			{
				foreach (var payloadLength in GetPayloadLengths())
				{
					testGroups.Add(new TestGroup()
					{
						HmacAlg = hashAlg,
						RandomLength = payloadLength,
						TestType = "AFT"
					});
				}
			}
			
			return Task.FromResult(testGroups);
		}

		private IEnumerable<int> GetPayloadLengths()
		{
			var domain = new MathDomain().AddSegment(new RangeDomainSegment(_random, 256, 2048, 8));

			var values = new List<int>();

			values.AddRangeIfNotNullOrEmpty(domain.GetValues(v => v < 512, 5, false));
			values.AddRangeIfNotNullOrEmpty(domain.GetValues(v => v < 1024, 2, false));
			values.AddRangeIfNotNullOrEmpty(domain.GetValues(1));

			return values.Shuffle().Take(5);
		}
	}
}