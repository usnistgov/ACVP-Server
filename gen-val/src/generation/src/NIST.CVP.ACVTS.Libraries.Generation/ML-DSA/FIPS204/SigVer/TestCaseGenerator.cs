using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer;

public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    
    // Set up to use 3 of each possible disposition, 10 is a placeholder
    public int NumberOfTestCasesToGenerate { get; private set; } = 10;

    public TestCaseGenerator(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
    {
        NumberOfTestCasesToGenerate = group.TestCaseExpectationProvider.ExpectationCount;

        return new GenerateResponse();
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new MLDSASignatureParameters
        {
            Deterministic = false,
            ParameterSet = group.ParameterSet,
            MessageLength = 1024,
            Disposition = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
            PrivateKey = group.PrivateKey
        };

        try
        {
            var result = await _oracle.GetMLDSAVerifyResultAsync(param);

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
