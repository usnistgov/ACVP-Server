using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, MLDSAVerificationResult>
{
    private readonly IOracle _oracle;

    public DeferredTestCaseResolver(IOracle oracle)
    {
        _oracle = oracle;
    }

    public async Task<MLDSAVerificationResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
    {
        var param = new MLDSASignatureParameters
        {
            ParameterSet = serverTestGroup.ParameterSet
        };

        var providedResult = new MLDSASignatureResult
        {
            PublicKey = serverTestGroup.PublicKey,
            Message = serverTestCase.Message,
            Signature = iutTestCase.Signature
        };

        // Need the try/catch here because the crypto will throw an exception if a bad signature is provided, when normally you'd just expect a "false" verification
        try
        {
            return await _oracle.CompleteDeferredMLDSASignatureAsync(param, providedResult);
        }
        catch (Exception ex)
        {
            return new MLDSAVerificationResult("Error validating signature");
        }
    }
}
