using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
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
