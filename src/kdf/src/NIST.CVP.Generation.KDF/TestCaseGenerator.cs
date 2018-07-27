using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.KDF
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            var param = new KdfParameters
            {
                Mode = group.KdfMode,
                MacMode = group.MacMode,
                CounterLocation = group.CounterLocation,
                CounterLength = group.CounterLength,
                KeyOutLength = group.KeyOutLength,
                ZeroLengthIv = group.ZeroLengthIv
            };

            try
            {
                if (isSample)
                {
                    var tmpResult = _oracle.GetDeferredKdfCase(param);
                    var result = _oracle.CompleteDeferredKdfCase(param, tmpResult);

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                    {
                        BreakLocation = tmpResult.BreakLocation,
                        FixedData = tmpResult.FixedData,
                        IV = tmpResult.Iv,
                        KeyIn = tmpResult.KeyIn,
                        KeyOut = result.KeyOut
                    });
                }
                else
                {
                    var result = _oracle.GetDeferredKdfCase(param);

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                    {
                        KeyIn = result.KeyIn,
                        IV = result.Iv
                    });
                }
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
