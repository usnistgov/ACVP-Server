using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 5;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
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

                ThisLogger.Debug($"Key is: {new BitString(result.KeyPair.PublicKey.Key).ToHex()}");
                ThisLogger.Debug($"I is: {result.I.ToHex()}");
                ThisLogger.Debug($"Seed is: {result.Seed.ToHex()}");
                
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
