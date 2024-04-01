using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.KeyGen;

public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 25;

    public TestCaseGenerator(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new MLKEMKeyGenParameters
        {
            ParameterSet = group.ParameterSet
        };
        
        try
        {
            var result = await _oracle.GetMLKEMKeyCaseAsync(param);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                SeedZ = result.SeedZ,
                SeedD = result.SeedD,
                EncapsulationKey = result.EncapsulationKey,
                DecapsulationKey = result.DecapsulationKey
            });
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
        }
    }
    
    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}
