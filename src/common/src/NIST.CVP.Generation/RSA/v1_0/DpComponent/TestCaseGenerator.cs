using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.RSA.v1_0.DpComponent
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; } = 1;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var failureTestIndexes = GetFailureIndexes(group.TotalTestCases, group.TotalFailingCases);
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponseSignature>()
            };

            for (var i = 0; i < group.TotalTestCases; i++)
            {
                var algoArrayResponse = new AlgoArrayResponseSignature();

                if (isSample)
                {
                    var param = new RsaDecryptionPrimitiveParameters
                    {
                        Modulo = group.Modulo,
                        TestPassed = !failureTestIndexes.Contains(i)
                    };

                    try
                    {
                        var result = await _oracle.GetRsaDecryptionPrimitiveAsync(param);
                        algoArrayResponse.Key = result.Key;
                        algoArrayResponse.CipherText = result.CipherText;
                        algoArrayResponse.PlainText = result.PlainText;
                        algoArrayResponse.TestPassed = param.TestPassed;
                    }
                    catch (Exception ex)
                    {
                        ThisLogger.Error(ex);
                        return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate test case");
                    }
                }
                else
                {
                    var param = new RsaDecryptionPrimitiveParameters
                    {
                        Modulo = group.Modulo,
                        TestPassed = !failureTestIndexes.Contains(i)
                    };

                    var result = await _oracle.GetDeferredRsaDecryptionPrimitiveAsync(param);
                    algoArrayResponse.CipherText = result.CipherText;
                    algoArrayResponse.TestPassed = param.TestPassed;
                }

                testCase.ResultsArray.Add(algoArrayResponse);
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();

        private int[] GetFailureIndexes(int total, int failing)
        {
            return Enumerable.Range(0, total).OrderBy(a => Guid.NewGuid()).Take(failing).ToArray();
        }
    }
}
