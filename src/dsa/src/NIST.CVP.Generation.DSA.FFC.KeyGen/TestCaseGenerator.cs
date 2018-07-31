using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                var param = new DsaKeyParameters
                {
                    DomainParameters = group.DomainParams
                };

                DsaKeyResult result = null;
                try
                {
                    result = _oracle.GetDsaKey(param);
                }
                catch (Exception ex)
                {
                    ThisLogger.Error(ex.StackTrace);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>("Error generating test case");
                }

                var testCase = new TestCase
                {
                    Key = result.Key
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
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
