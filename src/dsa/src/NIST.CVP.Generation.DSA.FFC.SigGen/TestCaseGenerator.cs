using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new DsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                MessageLength = group.L
            };

            try
            {
                DsaSignatureResult result = null;

                if (isSample)
                {
                    // If we are a sample, the group MUST have domain parameters
                    var keyParam = new DsaKeyParameters
                    {
                        DomainParameters = group.DomainParams
                    };

                    var keyResult = await _oracle.GetDsaKeyAsync(keyParam);

                    param.Key = keyResult.Key;
                    param.DomainParameters = keyParam.DomainParameters;

                    result = await _oracle.GetDsaSignatureAsync(param);
                    var testCase = new TestCase
                    {
                        Message = result.Message,
                        Key = param.Key,
                        Signature = result.Signature
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
                else
                {
                    result = await _oracle.GetDeferredDsaSignatureAsync(param);
                    var testCase = new TestCase
                    {
                        Message = result.Message
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate test case");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
