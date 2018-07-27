using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorG : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 2;

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

            var param = new DsaDomainParametersParameters
            {
                PQGenMode = group.PQGenMode,
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
                    var result = _oracle.GetDsaDomainParameters(param);
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
                    var result = _oracle.GetDsaPQ(param);
                    var testCase = new TestCase
                    {
                        Counter = result.Counter,
                        Seed = result.Seed,
                        P = result.P,
                        Q = result.Q
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
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
