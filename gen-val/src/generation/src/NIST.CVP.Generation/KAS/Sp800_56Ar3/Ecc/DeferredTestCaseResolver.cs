using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class DeferredTestCaseResolver : DeferredTestCaseResolverBase<TestGroup, TestCase, EccDomainParameters, EccKeyPair>
    {
        public DeferredTestCaseResolver(IOracle oracle) : base(oracle)
        {
        }

        protected override async Task<EccDomainParameters> GetDomainParameters(TestGroup serverTestGroup)
        {
            return await Oracle.GetEcdsaDomainParameterAsync(serverTestGroup.Curve);
        }
    }
}