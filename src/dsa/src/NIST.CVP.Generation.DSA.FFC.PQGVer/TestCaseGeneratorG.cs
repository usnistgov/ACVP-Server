using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseGeneratorG : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorG(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            // Get a PQ pair for the test case
            var pqParam = new DsaDomainParametersParameters
            {
                PQGenMode = PrimeGenMode.Probable,
                HashAlg = group.HashAlg,
                L = group.L,
                N = group.N
            };

            DsaDomainParametersResult pqResult = null;
            try
            {
                pqResult = _oracle.GetDsaPQ(pqParam);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Error generating PQ for test case");
            }

            // Get a G
            var reason = group.GTestCaseExpectationProvider.GetRandomReason();
            var gParam = new DsaDomainParametersParameters
            {
                Disposition = reason.GetName(),
                GGenMode = group.GGenMode,
                HashAlg = group.HashAlg,
                L = group.L,
                N = group.N
            };

            DsaDomainParametersResult gResult = null;
            try
            {
                gResult = _oracle.GetDsaG(gParam, pqResult);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Error generating G for test case");
            }

            // Assign values of the TestCase
            var testCase = new TestCase
            {
                P = pqResult.P,
                Q = pqResult.Q,
                Seed = pqResult.Seed,
                Counter = pqResult.Counter,
                Index = gResult.Index,
                Reason = reason.GetName(),
                TestPassed = reason.GetReason() == DsaGDisposition.None,
                G = gResult.G,
                H = gResult.H
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
