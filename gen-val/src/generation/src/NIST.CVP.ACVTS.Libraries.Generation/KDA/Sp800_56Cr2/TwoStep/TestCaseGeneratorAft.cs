using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.TwoStep
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate { get; private set; } = 25;

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var result = await _oracle.GetKdaAftTwoStepTestAsync(new KdaAftTwoStepParameters()
                {
                    KdfConfiguration = group.KdfConfiguration,
                    KdfConfigurationMultiExpand = group.KdfMultiExpansionConfiguration,
                    ZLength = group.ZLength
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    KdfParameter = result.KdfInputs,
                    FixedInfoPartyU = result.FixedInfoPartyU,
                    FixedInfoPartyV = result.FixedInfoPartyV,
                    Dkm = result.DerivedKeyingMaterial,

                    KdfMultiExpansionParameter = result.MultiExpansionInputs,
                    Dkms = result.MultiExpansionResult?.Results.Select(s => s.DerivedKey).ToList(),
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}
