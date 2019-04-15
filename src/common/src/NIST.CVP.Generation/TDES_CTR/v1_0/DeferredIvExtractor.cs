using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.TDES_CTR.v1_0
{
    public class DeferredIvExtractor : IDeferredTestCaseResolverAsync<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly IOracle _oracle;

        public DeferredIvExtractor(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<SymmetricCounterResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new TdesParameters
            {
                Direction = serverTestGroup.Direction,
                KeyingOption = serverTestGroup.KeyingOption
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
                var result = await _oracle.ExtractIvsAsync(param, fullParam);

                return new SymmetricCounterResult(
                    serverTestGroup.Direction.ToLower() == "encrypt" ? result.CipherText : result.PlainText,
                    result.IVs);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new SymmetricCounterResult($"Unable to compute IVs. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
