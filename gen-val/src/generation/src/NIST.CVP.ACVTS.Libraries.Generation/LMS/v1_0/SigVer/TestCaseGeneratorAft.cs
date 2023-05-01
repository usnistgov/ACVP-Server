using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer;

public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 4;
    
    public TestCaseGeneratorAft(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        try
        {
            var param = new LmsSignatureParameters
            {
                Disposition = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
                LmsMode = group.LmsMode,
                LmOtsMode = group.LmOtsMode,
                MessageLength = 1024,
                LmsKeyPair = group.KeyPair
            };

            var result = await _oracle.GetLmsVerifyResultAsync(param);

            var testCase = new TestCase
            {
                Message = result.VerifiedValue.Message,
                Reason = param.Disposition,
                TestPassed = result.Result,
                Signature = result.VerifiedValue.Signature
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
        }
    }
    
    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}
