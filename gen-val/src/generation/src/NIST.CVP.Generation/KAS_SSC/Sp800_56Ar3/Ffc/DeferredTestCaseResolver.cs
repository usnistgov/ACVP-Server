using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ffc
{
	public class DeferredTestCaseResolver : DeferredTestCaseResolverBase<TestGroup, TestCase, FfcDomainParameters, FfcKeyPair>
	{
		public DeferredTestCaseResolver(IOracle oracle) : base(oracle)
		{
		}

		protected override async Task<FfcDomainParameters> GetDomainParameters(TestGroup serverTestGroup)
		{
			// 186-* style domain parameters are not static, and need to be grabbed from testGroup.
			if (serverTestGroup.SafePrime == SafePrime.None)
			{
				return await Task.FromResult(serverTestGroup.FfcDomainParameters);
			}

			return SafePrimesFactory.GetDomainParameters(serverTestGroup.SafePrime);
		}
	}
}