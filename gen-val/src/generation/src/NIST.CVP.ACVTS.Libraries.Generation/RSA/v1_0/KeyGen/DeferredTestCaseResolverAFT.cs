using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen
{
    public class DeferredTestCaseResolverAFT : IDeferredTestCaseResolverAsync<TestGroup, TestCase, KeyResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolverAFT(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<KeyResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new RsaKeyParameters
            {
                HashAlg = serverTestGroup.HashAlg,
                KeyFormat = serverTestGroup.KeyFormat,
                KeyMode = RsaKeyGenAttributeConverter.GetPrimeGenFromSection(serverTestGroup.PrimeGenMode),
                Modulus = serverTestGroup.Modulo,
                PrimeTest = RsaKeyGenAttributeConverter.GetPrimeTestFromSection(serverTestGroup.PrimeTest),
                PublicExponentMode = serverTestGroup.PubExp,
                PublicExponent = serverTestGroup.FixedPubExp,
                Seed = iutTestCase.Seed,
                Standard = Fips186Standard.Fips186_4
            };

            var fullParam = new RsaKeyResult
            {
                BitLens = iutTestCase.Bitlens,
                AuxValues = new AuxiliaryResult
                {
                    XP = iutTestCase.XP.ToPositiveBigInteger(),
                    XP1 = iutTestCase.XP1.ToPositiveBigInteger(),
                    XP2 = iutTestCase.XP2.ToPositiveBigInteger(),
                    XQ = iutTestCase.XQ.ToPositiveBigInteger(),
                    XQ1 = iutTestCase.XQ1.ToPositiveBigInteger(),
                    XQ2 = iutTestCase.XQ2.ToPositiveBigInteger()
                },
                Key = iutTestCase.Key
            };

            try
            {
                var result = await _oracle.CompleteDeferredRsaKeyCaseAsync(param, fullParam);
                var verifyResult = await _oracle.GetRsaKeyVerifyAsync(new RsaKeyResult { Key = iutTestCase.Key });

                if (verifyResult.Result)
                {
                    return new KeyResult(result.Key, result.AuxValues);
                }

                return new KeyResult("Key is not a valid key");
            }
            catch (Exception)
            {
                return new KeyResult("Key is not a valid key");
            }
        }
    }
}
