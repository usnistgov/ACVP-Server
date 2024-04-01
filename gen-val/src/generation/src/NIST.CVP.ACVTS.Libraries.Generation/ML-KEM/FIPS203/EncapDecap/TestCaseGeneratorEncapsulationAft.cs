using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseGeneratorEncapsulationAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    
    public int NumberOfTestCasesToGenerate => 25;

    public TestCaseGeneratorEncapsulationAft(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        TestCase testCase = new TestCase();
        
        var keyParam = new MLKEMKeyGenParameters
        {
            ParameterSet = group.ParameterSet
        };

        try
        {
            var keyResult = await _oracle.GetMLKEMKeyCaseAsync(keyParam);

            testCase.EncapsulationKey = keyResult.EncapsulationKey;
            testCase.DecapsulationKey = keyResult.DecapsulationKey;
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
        }

        try
        {
            var encapParam = new MLKEMEncapsulationParameters
            {
                ParameterSet = group.ParameterSet,
                EncapsulationKey = testCase.EncapsulationKey
            };
            
            if (isSample)
            {

                var encapResult = await _oracle.GetMLKEMEncapCaseAsync(encapParam);

                testCase.Ciphertext = encapResult.Ciphertext;
                testCase.SharedKey = encapResult.SharedKey;
                testCase.SeedM = encapResult.SeedM;
            }
            else
            {
                var encapResult = await _oracle.GetMLKEMEncapDeferredCaseAsync(encapParam);

                testCase.SeedM = encapResult.SeedM;
            }
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error encapsulating: {ex.Message}");
        } 
        
        return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
    }
    
    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}
