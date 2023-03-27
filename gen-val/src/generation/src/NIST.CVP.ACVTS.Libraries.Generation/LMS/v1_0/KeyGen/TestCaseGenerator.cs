using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            // Trim the group down if the keys take a long time to generate
            NumberOfTestCasesToGenerate = AttributesHelper.GetLmsAttribute(group.LmsMode).H switch
            {
                5 => 5,
                10 => 4,
                15 => 3,
                20 => 2,
                25 => 1,
                _ => NumberOfTestCasesToGenerate
            };

            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            var param = new LmsKeyPairParameters
            {
                LmsMode = group.LmsMode,
                LmOtsMode = group.LmOtsMode
            };

            try
            {
                var result = await _oracle.GetLmsKeyCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Seed = result.Seed,
                    I = result.I,
                    PublicKey = new BitString(result.KeyPair.PublicKey.Key)
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
