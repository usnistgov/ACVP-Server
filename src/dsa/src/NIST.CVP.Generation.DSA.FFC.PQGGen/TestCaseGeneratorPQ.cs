using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorPQ : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorPQ(IOracle oracle)
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
                PQGenMode = group.PQGenMode,
                GGenMode = GeneratorGenMode.Unverifiable, // need to provide a GGenMode in order to get a potential pool value
                HashAlg = group.HashAlg,
                L = group.L,
                N = group.N
            };

            try
            {
                if (isSample)
                {
                    var result = await _oracle.GetDsaPQAsync(param);
                    var testCase = new TestCase
                    {
                        P = result.P,
                        Q = result.Q,
                        Seed = result.Seed,
                        Counter = result.Counter
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
                else
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase());
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate test case");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
