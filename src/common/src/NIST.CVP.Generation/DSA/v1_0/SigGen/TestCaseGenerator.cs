using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.DSA.v1_0.SigGen
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
                    param.Key = group.Key;
                    param.DomainParameters = group.DomainParams;

                    result = await _oracle.GetDsaSignatureAsync(param);
                    var testCase = new TestCase
                    {
                        Message = result.Message,
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
