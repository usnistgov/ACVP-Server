using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 10;

    public TestCaseGeneratorAft(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        try
        {
            var keyParam = new MLDSAKeyGenParameters { ParameterSet = group.ParameterSet };
            var keyResult = await _oracle.GetMLDSAKeyCaseAsync(keyParam);
            
            var param = new MLDSASignatureParameters
            {
                ParameterSet = group.ParameterSet,
                MessageLength = group.MessageLength.GetRandomValues(_ => true, 1).First(),
                Deterministic = group.Deterministic,
                PrivateKey = keyResult.PrivateKey
            };
            
            var result = await _oracle.GetMLDSASigGenCaseAsync(param);
            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                PrivateKey = keyResult.PrivateKey,
                Message = result.Message,
                Signature = result.Signature,
                Random = result.Rnd             // Null for deterministic = true
            });
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating signature: {ex.Message}");
        }
    }
    
    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}

