using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
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

            try
            {
                var result = await _oracle.GetAnsiX963KdfCaseAsync(param);
                
                var testCase = new TestCase
                {
                    Z = result.Z,
                    SharedInfo = result.SharedInfo,
                    KeyData = result.KeyOut
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
        
        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
