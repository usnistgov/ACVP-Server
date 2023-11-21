using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Shared.TwoStep
{
    public class TestCaseGeneratorVal : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly ITestCaseExpectationProvider<KdaTestCaseDisposition> _testCaseExpectationProvider;

        public TestCaseGeneratorVal(IOracle oracle, ITestCaseExpectationProvider<KdaTestCaseDisposition> testCaseExpectationProvider, int numberOfTestCasesToGenerate)
        {
            _oracle = oracle;
            _testCaseExpectationProvider = testCaseExpectationProvider;
            NumberOfTestCasesToGenerate = numberOfTestCasesToGenerate;
        }

        public int NumberOfTestCasesToGenerate { get; }
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            var disposition = _testCaseExpectationProvider.GetRandomReason();

            try
            {
                var result = await _oracle.GetKdaValTwoStepTestAsync(new KdaValTwoStepParameters()
                {
                    Disposition = disposition.GetReason(),
                    KdfConfiguration = group.KdfConfiguration,
                    KdfConfigurationMultiExpand = group.KdfMultiExpansionConfiguration,
                    ZLength = group.ZLength,
                    AuxSharedSecretLen = group.AuxSharedSecretLen
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    KdfParameter = result.KdfInputs,
                    FixedInfoPartyU = result.FixedInfoPartyU,
                    FixedInfoPartyV = result.FixedInfoPartyV,
                    Dkm = result.DerivedKeyingMaterial,
                    TestPassed = result.TestPassed,
                    KdfMultiExpansionParameter = result.MultiExpansionInputs,
                    Dkms = result.MultiExpansionResult?.Results.Select(s => s.DerivedKey).ToList(),
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(e.Message);
            }
        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}
