using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public abstract class TestGroupGeneratorBase<TTestGroup, TTestCase, TDomainParameters, TKeyPair> : ITestGroupGeneratorAsync<Parameters, TTestGroup, TTestCase>
		where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TDomainParameters : IDsaDomainParameters
		where TKeyPair : IDsaKeyPair
	{
		private static readonly string[] TestTypes =
		{
			"AFT",
			"VAL"
		};
		
		public async Task<List<TTestGroup>> BuildTestGroupsAsync(Parameters parameters)
		{
			List<TTestGroup> groups = new List<TTestGroup>();
			
			foreach (var scheme in parameters.Scheme.GetRegisteredSchemes())
			{
				foreach (var role in scheme.KasRole)
				{
					foreach (var dpGeneration in GetFilteredDpGeneration(parameters.DomainParameterGenerationMethods))
					{
						foreach (var testType in TestTypes)
						{
							groups.Add(new TTestGroup()
							{
								IsSample = parameters.IsSample,
								Scheme = scheme.Scheme,
								KasAlgorithm = scheme.UnderlyingAlgorithm,
								KasMode = KasMode.NoKdfNoKc,
								KasRole = role,
								TestType = testType,
								DomainParameterGenerationMode = dpGeneration,
								HashFunctionZ = parameters.HashFunctionZ
							});	
						}
					}
				}
			}

			await GenerateDomainParametersAsync(groups);
			return groups;
		}

		protected abstract KasDpGeneration[] GetFilteredDpGeneration(KasDpGeneration[] dpGeneration);
		protected abstract Task GenerateDomainParametersAsync(List<TTestGroup> testGroups);
	}
}