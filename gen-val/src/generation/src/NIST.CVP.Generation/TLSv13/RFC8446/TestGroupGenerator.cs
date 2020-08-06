using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
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

			var payloadLengthQueue = new ShuffleQueue<int>(GetPayloadLengths());
			var runningModeQueue = new ShuffleQueue<TlsModes1_3>(parameters.RunningModes.ToList());

			var maxCount = new[] {payloadLengthQueue.OriginalListCount, runningModeQueue.OriginalListCount}.Max();
			
			foreach (var hashAlg in parameters.HmacAlg)
			{
				for (var i = 0; i < maxCount; i++)
				{
					testGroups.Add(new TestGroup()
					{
						HmacAlg = hashAlg,
						RandomLength = payloadLengthQueue.Pop(),
						RunningMode = runningModeQueue.Pop(),
						TestType = "AFT"
					});
				}
			}
			
			return Task.FromResult(testGroups);
		}

		private List<int> GetPayloadLengths()
		{
			var domain = new MathDomain().AddSegment(new RangeDomainSegment(_random, 256, 2048, 8));

			var values = new List<int>();

			values.AddRangeIfNotNullOrEmpty(domain.GetValues(v => v < 512, 5, false));
			values.AddRangeIfNotNullOrEmpty(domain.GetValues(v => v < 1024, 2, false));
			values.AddRangeIfNotNullOrEmpty(domain.GetValues(1));

			return values.Shuffle().Take(5).ToList();
		}
	}
}