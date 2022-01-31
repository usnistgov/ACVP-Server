using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new KdfParameters
            {
                IvLength = group.IvLength,
                KeyInLength = group.KeyInLength,
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
                    var tmpResult = await _oracle.GetDeferredKdfCaseAsync(param);
                    var result = await _oracle.CompleteDeferredKdfCaseAsync(param, tmpResult);

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
                    var result = await _oracle.GetDeferredKdfCaseAsync(param);

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                    {
                        KeyIn = result.KeyIn,
                        IV = result.Iv
                    });
                }
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
