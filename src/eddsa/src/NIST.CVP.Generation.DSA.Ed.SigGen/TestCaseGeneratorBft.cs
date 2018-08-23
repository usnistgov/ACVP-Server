using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.Ed.SigGen
{
    public class TestCaseGeneratorBft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 1024;

        private int _bit = -1;

        public TestCaseGeneratorBft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 32;
            }

            var param = new EddsaSignatureParameters
            {
                Curve = group.Curve,
                PreHash = group.PreHash,
                Key = group.KeyPair,
                Message = group.Message,
                Bit = _bit++
            };

            try
            {
                TestCase testCase = null;
                EddsaSignatureResult result = null;
                if (isSample)
                {
                    result = await _oracle.GetEddsaSignatureBitFlipAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message,
                        Context = result.Context,
                        Signature = result.Signature
                    };

                }
                else
                {
                    result = await _oracle.GetDeferredEddsaSignatureBitFlipAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message,
                        Context = result.Context
                    };
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
