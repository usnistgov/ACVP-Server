using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class DeferredAftTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, MLKEMDecapsulationResult>
{
    private readonly IOracle _oracle;

    public DeferredAftTestCaseResolver(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<MLKEMDecapsulationResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
    {
        var param = new MLKEMDecapsulationParameters
        {
            ParameterSet = serverTestGroup.ParameterSet,
            DecapsulationKey = serverTestCase.DecapsulationKey
        };
        
        var providedResult = new MLKEMEncapsulationResult
        {
            Ciphertext = iutTestCase.Ciphertext
        };

        return await _oracle.CompleteDeferredMLKEMEncapsulationAsync(param, providedResult);
    }
}
