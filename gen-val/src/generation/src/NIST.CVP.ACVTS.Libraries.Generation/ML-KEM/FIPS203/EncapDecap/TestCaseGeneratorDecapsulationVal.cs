using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseGeneratorDecapsulationVal : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    
    // Set up to use 5 of each possible disposition
    public int NumberOfTestCasesToGenerate => 10;

    public TestCaseGeneratorDecapsulationVal(IOracle oracle)
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

        var param = new MLKEMDecapsulationParameters
        {
            ParameterSet = group.ParameterSet,
            Disposition = group.DecapsulationExpectationProvider.GetRandomReason(),
            EncapsulationKey = testCase.EncapsulationKey,
            DecapsulationKey = testCase.DecapsulationKey,
        };

        try
        {
            var result = await _oracle.GetMLKEMDecapCaseAsync(param);
            
            testCase.Reason = EnumHelpers.GetEnumDescriptionFromEnum(param.Disposition); 
            testCase.SharedKey = result.SharedKey;
            testCase.Ciphertext = result.Ciphertext;

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
