using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
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
            var task = Oracle.GetEcdsaDomainParameterAsync(new EcdsaCurveParameters() {Curve = serverTestGroup.Curve});
            var result = await task;
            
            return result.EccDomainParameters;
        }
    }
}