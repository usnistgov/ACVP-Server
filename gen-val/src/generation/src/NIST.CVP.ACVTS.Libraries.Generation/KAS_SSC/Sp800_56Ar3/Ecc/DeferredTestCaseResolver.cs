using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3.Ecc
{
    public class DeferredTestCaseResolver : DeferredTestCaseResolverBase<TestGroup, TestCase, EccDomainParameters, EccKeyPair>
    {
        public DeferredTestCaseResolver(IOracle oracle) : base(oracle)
        {
        }

        protected override async Task<EccDomainParameters> GetDomainParameters(TestGroup serverTestGroup)
        {
            var task = Oracle.GetEcdsaDomainParameterAsync(new EcdsaCurveParameters() { Curve = serverTestGroup.Curve });
            var result = await task;

            return result.EccDomainParameters;
        }
    }
}
