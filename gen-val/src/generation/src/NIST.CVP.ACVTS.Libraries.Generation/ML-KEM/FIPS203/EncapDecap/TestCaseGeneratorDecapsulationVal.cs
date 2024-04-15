using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseGeneratorDecapsulationVal : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    
    // Set up to use 3 of each possible disposition, 10 is a placeholder because we need the group
    public int NumberOfTestCasesToGenerate { get; private set; } = 10;

    public TestCaseGeneratorDecapsulationVal(IOracle oracle)
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
        var param = new MLKEMDecapsulationParameters
        {
            ParameterSet = group.ParameterSet,
            Disposition = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
            EncapsulationKey = group.EncapsulationKey,
            DecapsulationKey = group.DecapsulationKey
        };

        try
        {
            var result = await _oracle.GetMLKEMDecapCaseAsync(param);

            var testCase = new TestCase
            {
                Reason = param.Disposition, 
                SharedKey = result.SharedKey,
                Ciphertext = result.Ciphertext
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
