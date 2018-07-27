using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.SNMP
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 20;
            }

            var param = new SnmpKdfParameters
            {
                PasswordLength = group.PasswordLength,
                EngineId = group.EngineId
            };

            SnmpKdfResult result = null;
            try
            {
                result = _oracle.GetSnmpKdfCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            var testCase = new TestCase
            {
                Password = result.Password,
                SharedKey = result.SharedKey
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        public Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
