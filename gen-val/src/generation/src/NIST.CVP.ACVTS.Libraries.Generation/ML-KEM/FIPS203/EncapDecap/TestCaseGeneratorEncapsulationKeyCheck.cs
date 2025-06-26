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

public class TestCaseGeneratorEncapsulationKeyCheck : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    
    public int NumberOfTestCasesToGenerate => 10;

    public TestCaseGeneratorEncapsulationKeyCheck(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var keyParam = new MLKEMKeyGenParameters
        {
            ParameterSet = group.ParameterSet,
            EncapDisposition = group.EncapsulationKeyExpectationProvider.GetRandomReason(),
        };

        try
        {
            var keyResult = await _oracle.GetMLKEMEncapKeyCheckCaseAsync(keyParam);

            var testCase =  new TestCase
            {
                EncapsulationKey = keyResult.EncapsulationKey,
                DecapsulationKey = keyResult.DecapsulationKey,
                Reason = EnumHelpers.GetEnumDescriptionFromEnum(keyParam.EncapDisposition),
                TestPassed = keyParam.EncapDisposition == MLKEMEncapsulationKeyDisposition.None
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
