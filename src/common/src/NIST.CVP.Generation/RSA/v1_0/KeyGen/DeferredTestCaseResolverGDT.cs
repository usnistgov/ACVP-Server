using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public class DeferredTestCaseResolverGDT : IDeferredTestCaseResolverAsync<TestGroup, TestCase, KeyResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolverGDT(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<KeyResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new RsaKeyParameters
            {
                HashAlg = serverTestGroup.HashAlg,
                KeyFormat = serverTestGroup.KeyFormat,
                KeyMode = serverTestGroup.PrimeGenMode,
                Modulus = serverTestGroup.Modulo,
                PrimeTest = serverTestGroup.PrimeTest,
                PublicExponentMode = serverTestGroup.PubExp,
                PublicExponent = serverTestGroup.FixedPubExp,
                Seed = iutTestCase.Seed,
                Standard = Fips186Standard.Fips186_4
            };

            var verifyResult = await _oracle.GetRsaKeyVerifyAsync(new RsaKeyResult{Key = iutTestCase.Key});

            if (verifyResult.Result)
            {
                return new KeyResult(iutTestCase.Key, null);
            }
            
            return new KeyResult("Key is not a valid key");
        }
    }
}