using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorG : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorG(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            var param = new DsaDomainParametersParameters
            {
                PQGenMode = PrimeGenMode.Probable,
                GGenMode = group.GGenMode,
                HashAlg = group.HashAlg,
                L = group.L,
                N = group.N
            };

            try
            {
                if (isSample)
                {
                    // Needs PQ and G
                    var result = await _oracle.GetDsaDomainParametersAsync(param);
                    var testCase = new TestCase
                    {
                        Counter = result.Counter,
                        Seed = result.Seed,
                        P = result.P,
                        Q = result.Q,
                        G = result.G,
                        Index = result.Index
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
                else
                {
                    // Needs PQ
                    var result = await _oracle.GetDsaDomainParametersAsync(param);
                    var testCase = new TestCase
                    {
                        Counter = result.Counter,
                        Seed = result.Seed,
                        P = result.P,
                        Q = result.Q,
                        // Do not save G, client is responsible for generating it
                        Index = result.Index
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
