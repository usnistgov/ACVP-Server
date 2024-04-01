using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestCaseGeneratorGdt : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 10;

    public TestCaseGeneratorGdt(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new MLDSASignatureParameters
        {
            ParameterSet = group.ParameterSet,
            MessageLength = 1024,
            Deterministic = group.Deterministic,     // This doesn't make a noticeable difference but can still be tested
            PrivateKey = group.PrivateKey            // Only used for isSample = true
        };
        
        try
        {
            if (isSample)
            {
                var result = await _oracle.GetMLDSASigGenCaseAsync(param);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = result.Message,
                    Signature = result.Signature
                });
            }
            else
            {
                var result = await _oracle.GetMLDSASigGenDeferredCaseAsync(param);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = result.Message
                });
            }
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating signature: {ex.Message}");
        }
    }
    
    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}
