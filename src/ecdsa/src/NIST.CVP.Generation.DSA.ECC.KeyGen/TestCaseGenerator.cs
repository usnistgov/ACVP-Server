using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;

                var param = new EcdsaKeyParameters
                {
                    Curve = group.Curve
                };

                EcdsaKeyResult result = null;
                try
                {
                    result = _oracle.GetEcdsaKey(param);
                }
                catch (Exception ex)
                {
                    ThisLogger.Error(ex.StackTrace);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    KeyPair = result.Key
                });
            }
            else
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase());
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
