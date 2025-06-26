using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseGeneratorDecapsulationKeyCheck : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    
    public int NumberOfTestCasesToGenerate => 10;

    public TestCaseGeneratorDecapsulationKeyCheck(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var keyParam = new MLKEMKeyGenParameters
        {
            ParameterSet = group.ParameterSet,
            DecapDisposition = group.DecapsulationKeyExpectationProvider.GetRandomReason(),
        };

        try
        {
            var keyResult = await _oracle.GetMLKEMDecapKeyCheckCaseAsync(keyParam);

            var testCase =  new TestCase
            {
                EncapsulationKey = keyResult.EncapsulationKey,
                DecapsulationKey = keyResult.DecapsulationKey,
                Reason = EnumHelpers.GetEnumDescriptionFromEnum(keyParam.DecapDisposition),
                TestPassed = keyParam.DecapDisposition == MLKEMDecapsulationKeyDisposition.None
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
        }
    }
    
    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}
