using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.KeyGen
{
    public class DeferredTestCaseResolverGdt : IDeferredTestCaseResolverAsync<TestGroup, TestCase, KeyResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolverGdt(IOracle oracle)
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

            var verifyResult = await _oracle.GetRsaKeyVerifyAsync(new RsaKeyResult { Key = iutTestCase.Key });

            if (verifyResult.Result)
            {
                return new KeyResult(iutTestCase.Key, null);
            }

            return new KeyResult("Key is not a valid key");
        }
    }
}
