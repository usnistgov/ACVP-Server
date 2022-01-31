using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigGen
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
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
                    param.IsMessageRandomized = group.IsMessageRandomized;

                    result = await _oracle.GetDsaSignatureAsync(param);
                    var testCase = new TestCase
                    {
                        Message = result.Message,
                        RandomValue = result.RandomValue,
                        RandomValueLen = result.RandomValue?.BitLength ?? 0,
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
