using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer
{
    public class TestCaseGeneratorPQ : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorPQ(IOracle oracle)
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
            var reason = group.PQTestCaseExpectationProvider.GetRandomReason();
            var param = new DsaDomainParametersParameters
            {
                Disposition = reason.GetName(),
                PQGenMode = group.PQGenMode,
                GGenMode = GeneratorGenMode.Unverifiable, // need to provide a GGenMode in order to get a potential pool value
                HashAlg = group.HashAlg,
                L = group.L,
                N = group.N
            };

            try
            {
                var result = await _oracle.GetDsaPQAsync(param);

                var testCase = new TestCase
                {
                    Reason = param.Disposition,
                    P = result.P != 0 ? new BitString(result.P, group.L) : null,
                    Q = result.Q != 0 ? new BitString(result.Q, group.N) : null,
                    Seed = result.Seed,
                    Counter = result.Counter,
                    TestPassed = reason.GetReason() == DsaPQDisposition.None
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating test case. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
