using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent
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
            
            var rsaDpParams = new RsaDecryptionPrimitiveParameters()
            {
                Modulo = group.Modulo,
                Mode = group.KeyMode,
                Disposition = reason.GetName()
            };
            
            try
            {
                var dpResult = await _oracle.GetRsaDecryptionPrimitiveSp800B56Br2Async(rsaDpParams);

                SetTestCaseData(testCase, dpResult);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate test case");
            }
            
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private static void SetTestCaseData(TestCase testCase, RsaDecryptionPrimitiveResult dpResult)
        {
            testCase.CipherText = dpResult.CipherText;
            testCase.PlainText = dpResult.PlainText;
            testCase.TestPassed = dpResult.TestPassed;
            testCase.N = dpResult.Key.PubKey.N;
            testCase.E = dpResult.Key.PubKey.E;
            testCase.Q = dpResult.Key.PrivKey.Q;
            testCase.P = dpResult.Key.PrivKey.P;
            testCase.Dmp1 = dpResult.Key.PrivKey.DMP1;
            testCase.Dmq1 = dpResult.Key.PrivKey.DMQ1;
            testCase.Iqmp = dpResult.Key.PrivKey.IQMP;
            testCase.D = dpResult.Key.PrivKey.D;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
