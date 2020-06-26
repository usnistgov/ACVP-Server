using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
	{
		private readonly string[] _testTypes = { "AFT", "VAL" };
		
		public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
		{
			var groups = new List<TestGroup>();

			foreach (var testType in _testTypes)
			{
				foreach (var fixedInfoEncoding in parameters.FixedInfoEncoding)
				{
					foreach (var auxFunction in parameters.AuxFunctions)
					{
						foreach (var saltMethod in GetSaltingMethods(auxFunction))
						{
							groups.Add(new TestGroup()
							{
								KdfConfiguration = new OneStepConfiguration()
								{
									L = parameters.L,
									AuxFunction = auxFunction.AuxFunctionName,
									SaltMethod = saltMethod,
									SaltLen = GetSaltLen(auxFunction),
									FixedInfoEncoding = fixedInfoEncoding,
									FixedInfoPattern = parameters.FixedInfoPattern
								},
								TestType = testType,
								IsSample = parameters.IsSample
							});
						}
					}
				}				
			}
			
			return Task.FromResult(groups);
		}

		private int GetSaltLen(AuxFunction auxFunction)
		{
            switch (auxFunction.AuxFunctionName)
            {
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D224:
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D224:
	                return 224;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D256:
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T256:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D256:
	                return 256;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D384:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D384:
                    return 384;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D512:
	                return 512;
                case KasKdfOneStepAuxFunction.KMAC_128:
                    return 128;
                case KasKdfOneStepAuxFunction.KMAC_256:
                    return 256;
            }

            return 0;
		}

		private List<MacSaltMethod> GetSaltingMethods(AuxFunction auxFunction)
		{
			var results = new List<MacSaltMethod>();

			if (auxFunction.MacSaltMethods == null || auxFunction.MacSaltMethods.Length == 0)
			{
				results.Add(MacSaltMethod.None);
			}
			
			results.AddRangeIfNotNullOrEmpty(auxFunction.MacSaltMethods);
			
			return results;
		}
	}
}