using System;
 using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 15;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 10;
            }
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var testCase = new TestCase();
            var reason = group.TestCaseExpectationProvider.GetRandomReason();
            
            var rsaParams = new RsaSignaturePrimitiveParameters()
            {
                Modulus = group.Modulo,
                KeyFormat = group.KeyMode,
                PublicExponent = group.PublicExponentMode == PublicExponentModes.Fixed ? group.PublicExponent : null,
                Disposition = reason.GetName()
            };

            try
            {
                var result = await _oracle.GetRsaSignaturePrimitiveV2_0Async(rsaParams);

                SetTestCaseData(result, testCase, group.Modulo);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate test case");
            }
            
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private void SetTestCaseData(RsaSignaturePrimitiveResult result, TestCase testCase, int modulo)
        {
            testCase.Signature = result.Signature?.PadToModulusMsb(modulo);
            testCase.Message = result.Message;
            testCase.TestPassed = result.ShouldPass;
            testCase.N = result.Key.PubKey.N;
            testCase.E = result.Key.PubKey.E;
            testCase.P = result.Key.PrivKey.P;
            testCase.Q = result.Key.PrivKey.Q;
            testCase.D = result.Key.PrivKey.D;
            testCase.Dmp1 = result.Key.PrivKey.DMP1;
            testCase.Dmq1 = result.Key.PrivKey.DMQ1;
            testCase.Iqmp = result.Key.PrivKey.IQMP;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
