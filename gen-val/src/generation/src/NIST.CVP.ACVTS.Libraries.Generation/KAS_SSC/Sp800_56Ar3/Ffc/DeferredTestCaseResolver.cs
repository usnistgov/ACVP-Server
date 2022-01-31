using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3.Ffc
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
