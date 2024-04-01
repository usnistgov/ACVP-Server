using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.KeyGen;

public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 25;   // Set high to try to trigger failures in RejBoundedPoly which needs extra sampling from SHAKE ~20% of the time for ML_DSA_65, compared to ~0.4% for other parameter sets

    public TestCaseGenerator(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new MLDSAKeyGenParameters
        {
            ParameterSet = group.ParameterSet
        };
        
        try
        {
            var result = await _oracle.GetMLDSAKeyCaseAsync(param);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Seed = result.Seed,
                PublicKey = result.PublicKey,
                PrivateKey = result.PrivateKey
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
