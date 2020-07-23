using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfHkdf;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS_KDF.Hkdf
{
	public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
	{
		private readonly string[] _testTypes = { "AFT", "VAL" };
		
		public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
		{
			var groups = new List<TestGroup>();

			foreach (var testType in _testTypes)
			{
				foreach (var fixedInfoEncoding in parameters.Encoding)
				{
					foreach (var hmacAlg in parameters.HmacAlg)
					{
						foreach (var saltMethod in parameters.MacSaltMethods)
						{
							foreach (var zLength in GetZs(parameters.Z))
							{
								groups.Add(new TestGroup()
								{
									KdfConfiguration = new HkdfConfiguration()
									{
										L = parameters.L,
										HmacAlg = hmacAlg,
										SaltMethod = saltMethod,
										SaltLen = GetSaltLen(hmacAlg),
										FixedInfoEncoding = fixedInfoEncoding,
										FixedInfoPattern = parameters.FixedInfoPattern
									},
									TestType = testType,
									IsSample = parameters.IsSample,
									ZLength = zLength
								});
							}
						}
					}
				}				
			}
			
			return Task.FromResult(groups);
		}

		private int GetSaltLen(HashFunctions hmacAlg)
		{
            switch (hmacAlg)
            {
                case HashFunctions.Sha2_d224:
                case HashFunctions.Sha2_d512t224:
                case HashFunctions.Sha3_d224:
	                return 224;
                case HashFunctions.Sha2_d256:
                case HashFunctions.Sha2_d512t256:
                case HashFunctions.Sha3_d256:
	                return 256;
                case HashFunctions.Sha2_d384:
                case HashFunctions.Sha3_d384:
                    return 384;
                case HashFunctions.Sha2_d512:
                case HashFunctions.Sha3_d512:
	                return 512;
            }

            return 0;
		}
		
		private List<int> GetZs(MathDomain z)
		{
			var values = new List<int>();

			values.AddRange(z.GetValues(i => i < 1024, 10, false));
			values.AddRange(z.GetValues(i => i < 4098, 5, false));
			values.AddRange(z.GetValues(i => i < 8196, 2, false));
			values.AddRange(z.GetValues(1));
			
			return values.Shuffle().Take(5).ToList();
		}
	}
}