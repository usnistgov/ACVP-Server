using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.KeyConfirmation;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            NumberOfTestCasesToGenerate = group.KasKcExpectationProvider.ExpectationCount;
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var disposition = group.KasKcExpectationProvider.GetRandomReason();
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
