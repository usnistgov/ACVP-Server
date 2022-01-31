using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.KeyConfirmation;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly ITestCaseExpectationProvider<KasKcDisposition> _testCaseExpectationProvider;

        public TestCaseGeneratorAft(IOracle oracle, ITestCaseExpectationProvider<KasKcDisposition> testCaseExpectationProvider)
        {
            _oracle = oracle;
            _testCaseExpectationProvider = testCaseExpectationProvider;
            NumberOfTestCasesToGenerate = _testCaseExpectationProvider.ExpectationCount;
        }

        public int NumberOfTestCasesToGenerate { get; }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var disposition = _testCaseExpectationProvider.GetRandomReason().GetReason();
                var result = await _oracle.GetKasKcAftTestAsync(new KasKcAftParameters()
                {
                    Disposition = disposition,
                    KasRole = group.KasRole,
                    KeyLen = group.KeyLen,
                    MacLen = group.MacLen,
                    KeyConfirmationDirection = group.KeyConfirmationDirection,
                    KeyConfirmationRole = group.KeyConfirmationRole,
                    KeyAgreementMacType = group.KeyAgreementMacType,
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Tag = result.Tag,
                    MacData = result.MacData,
                    MacKey = result.MacKey,
                    MacDataIut = result.IutMacDataContribution,
                    MacDataServer = result.ServerMacDataContribution,
                    Disposition = disposition
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
        }
    }
}
