using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;

public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 10;

    public TestCaseGenerator(IOracle oracle)
    {
        _oracle = oracle;
    }

    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new SLHDSAKeyGenParameters { SlhdsaParameterSet = group.ParameterSet };

        try
        {
            var result = await _oracle.GetSLHDSAKeyCaseAsync(param);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                SKSeed = result.SKSeed,
                SKPrf = result.SKPrf,
                PKSeed = result.PKSeed,
                PrivateKey = result.PrivateKey,
                PublicKey = result.PublicKey
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
