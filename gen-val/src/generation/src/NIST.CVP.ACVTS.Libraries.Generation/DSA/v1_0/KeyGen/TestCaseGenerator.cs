using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            if (isSample)
            {
                var param = new DsaKeyParameters
                {
                    DomainParameters = group.DomainParams
                };

                try
                {
                    var result = await _oracle.GetDsaKeyAsync(param);

                    var testCase = new TestCase
                    {
                        Key = result.Key
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
                catch (Exception ex)
                {
                    ThisLogger.Error(ex.StackTrace);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>("Error generating test case");
                }
            }
            else
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase());
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
