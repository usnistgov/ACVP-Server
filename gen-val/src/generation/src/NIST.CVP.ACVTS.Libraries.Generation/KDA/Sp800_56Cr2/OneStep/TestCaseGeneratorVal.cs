using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStep
{
    public class TestCaseGeneratorVal : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorVal(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            NumberOfTestCasesToGenerate = group.KdaExpectationProvider.ExpectationCount;
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var result = await _oracle.GetKdaValOneStepTestAsync(new KdaValOneStepParameters()
                {
                    Disposition = group.KdaExpectationProvider.GetRandomReason(),
                    OneStepConfiguration = group.KdfConfiguration,
                    ZLength = group.ZLength
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    KdfParameter = result.KdfInputs,
                    FixedInfoPartyU = result.FixedInfoPartyU,
                    FixedInfoPartyV = result.FixedInfoPartyV,
                    Dkm = result.DerivedKeyingMaterial,
                    TestPassed = result.TestPassed
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
