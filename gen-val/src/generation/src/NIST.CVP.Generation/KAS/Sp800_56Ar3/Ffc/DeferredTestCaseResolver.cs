using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ffc
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
            
            // Safe prime groups *could* be grabbed from testGroup, but are not serialized due to them being static. Just grab from an orleans grain
            return await Oracle.GetSafePrimeGroupsDomainParameterAsync(serverTestGroup.SafePrime);
        }
    }
}