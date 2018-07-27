using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseGeneratorPQ : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorPQ(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            var reason = group.PQTestCaseExpectationProvider.GetRandomReason();
            var param = new DsaDomainParametersParameters
            {
                Disposition = reason.GetName(),
                PQGenMode = group.PQGenMode,
                HashAlg = group.HashAlg,
                L = group.L,
                N = group.N
            };

            DsaDomainParametersResult result = null;
            try
            {
                result = _oracle.GetDsaPQ(param);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating test case. {ex.Message}");
            }

            var testCase = new TestCase
            {
                Reason = param.Disposition,
                P = result.P,
                Q = result.Q,
                Seed = result.Seed,
                Counter = result.Counter,
                TestPassed = reason.GetReason() == DsaPQDisposition.None
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
