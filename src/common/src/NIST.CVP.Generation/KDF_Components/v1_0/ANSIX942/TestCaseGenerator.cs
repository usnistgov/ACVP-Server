using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate => 15;
        private readonly IOracle _oracle;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AnsiX942Parameters
            {
                KeyLen = group.KeyLen,
                HashAlg = group.HashAlg,
                OtherIntoLen = group.OtherInfoLen,
                ZzLen = group.ZzLen,
                KdfMode = group.KdfType
            };

            try
            {
                var result = await _oracle.GetAnsiX942KdfCaseAsync(param);
                
                var testCase = new TestCase
                {
                    Zz = result.Zz,
                    OtherInfo = result.OtherInfo,
                    DerivedKey = result.DerivedKey
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
