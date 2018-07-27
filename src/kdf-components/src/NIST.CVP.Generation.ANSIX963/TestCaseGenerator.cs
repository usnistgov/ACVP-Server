using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.ANSIX963
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

            var param = new AnsiX963Parameters
            {
                FieldSize = group.FieldSize,
                SharedInfoLength = group.SharedInfoLength,
                HashAlg = group.HashAlg,
                KeyDataLength = group.KeyDataLength
            };

            AnsiX963KdfResult result = null;
            try
            {
                result = _oracle.GetAnsiX963KdfCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            var testCase = new TestCase
            {
                Z = result.Z,
                SharedInfo = result.SharedInfo,
                KeyData = result.KeyOut
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
