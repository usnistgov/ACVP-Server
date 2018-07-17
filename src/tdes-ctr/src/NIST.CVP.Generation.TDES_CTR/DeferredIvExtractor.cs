using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class DeferredIvExtractor : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly IOracle _oracle;

        public DeferredIvExtractor(IOracle oracle)
        {
            _oracle = oracle;
        }

        public SymmetricCounterResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new TdesParameters
            {
                Direction = serverTestGroup.Direction,
                KeyingOption = TdesHelpers.GetKeyingOptionFromNumberOfKeys(serverTestGroup.NumberOfKeys)
            };

            var fullParam = new TdesResult
            {
                Key = serverTestCase.Key,
                Iv = serverTestCase.Iv,
                PlainText = serverTestGroup.Direction.ToLower() == "encrypt"
                    ? serverTestCase.PlainText
                    : iutTestCase.PlainText,
                CipherText = serverTestGroup.Direction.ToLower() == "decrypt"
                    ? serverTestCase.CipherText
                    : iutTestCase.CipherText
            };

            try
            {
                var result = _oracle.ExtractIvs(param, fullParam);

                return new SymmetricCounterResult(
                    serverTestGroup.Direction.ToLower() == "encrypt" ? result.CipherText : result.PlainText,
                    result.IVs);
            }
            catch (Exception ex)
            {
                return new SymmetricCounterResult($"Unable to compute IVs. {ex.Message}");
            }
        }
    }
}
