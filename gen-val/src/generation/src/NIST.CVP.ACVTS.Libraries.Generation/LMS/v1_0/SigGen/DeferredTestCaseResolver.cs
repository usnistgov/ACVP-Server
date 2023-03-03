using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;

public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, LmsVerificationResult>
{
    private readonly IOracle _oracle;

    public DeferredTestCaseResolver(IOracle oracle)
    {
        _oracle = oracle;
    }

    public async Task<LmsVerificationResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
    {
        var iutTestGroup = iutTestCase.ParentGroup;
        var param = new LmsSignatureParameters();

        var providedResult = new LmsSignatureResult
        {
            PublicKey = iutTestGroup.PublicKey,
            Message = serverTestCase.Message,
            Signature = iutTestCase.Signature
        };

        // Need the try/catch here because the crypto will throw an exception if a bad signature is provided, when normally you'd just expect a "false" verification
        try
        {
            return await _oracle.CompleteDeferredLmsSignatureAsync(param, providedResult);
        }
        catch (Exception ex)
        {
            return new LmsVerificationResult(ex.Message);
        }
    }
}
