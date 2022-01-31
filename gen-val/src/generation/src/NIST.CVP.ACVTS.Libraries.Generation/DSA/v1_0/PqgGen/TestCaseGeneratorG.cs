using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen
{
    public class TestCaseGeneratorG : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorG(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
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
                var result = await _oracle.GetDsaDomainParametersAsync(param);
                TestCase testCase;

                if (isSample)
                {
                    // Needs PQ and G
                    testCase = new TestCase
                    {
                        Counter = result.Counter,
                        Seed = result.Seed,
                        P = result.P,
                        Q = result.Q,
                        G = result.G,
                        Index = result.Index
                    };

                }
                else
                {
                    // Needs PQ
                    testCase = new TestCase
                    {
                        Counter = result.Counter,
                        Seed = result.Seed,
                        P = result.P,
                        Q = result.Q,
                        // Do not save G, client is responsible for generating it
                        Index = result.Index
                    };
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
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
