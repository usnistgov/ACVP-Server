using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestCaseGeneratorPoolAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    public int NumberOfTestCasesToGenerate { get; set; } = 5;

    public TestCaseGeneratorPoolAft(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
    {
        // For ML-DSA-87, Total Rejection cases, lower the amount of test cases to 2.
        // These are really hard to generate for the server, so there is not many available in the pools.
        if (group.ParameterSet == DilithiumParameterSet.ML_DSA_87 && group.CornerCase == MLDSASignatureCornerCase.TotalRejection)
        {
            NumberOfTestCasesToGenerate = 2;
        }

        return new GenerateResponse();
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        try
        {
            var param = new MLDSASignatureParameters
            {
                ParameterSet = group.ParameterSet,
                MessageLength = 256,                            // Assumption that messageLength is supported from TestGroupGeneratorPoolAft
                Deterministic = group.Deterministic,            // Always true
                SignatureInterface = group.SignatureInterface,  // Always internal
                CornerCase = group.CornerCase
            };
            
            var poolResult = await _oracle.GetMLDSASigGenCornerCaseAsync(param);
            
            // poolResult contains a seed and message without returning the private key and signature, they need to be generated separately from a special grain
            var fullResult = await _oracle.CompleteMLDSASigGenCornerCaseAsync(param, poolResult);
            
            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                PrivateKey = fullResult.PrivateKey,
                Message = poolResult.Message,
                Signature = fullResult.Signature
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
