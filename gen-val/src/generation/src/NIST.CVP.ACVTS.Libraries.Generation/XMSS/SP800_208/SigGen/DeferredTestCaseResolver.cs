using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;

public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, XmssVerificationResult>
{
    private readonly IOracle _oracle;

    public DeferredTestCaseResolver(IOracle oracle)
    {
        _oracle = oracle;
    }

    public async Task<XmssVerificationResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
    {
        var iutTestGroup = iutTestCase.ParentGroup;
        var param = new XmssSignatureParameters();

        var providedResult = new XmssSignatureResult
        {
            PublicKey = iutTestGroup.PublicKey,
            Message = serverTestCase.Message,
            Signature = iutTestCase.Signature
        };

        // Need the try/catch here because the crypto will throw an exception if a bad signature is provided, when normally you'd just expect a "false" verification
        try
        {
            return await _oracle.CompleteDeferredXmssSignatureAsync(param, providedResult);
        }
        catch (Exception ex)
        {
            return new XmssVerificationResult(ex.Message);
        }
    }
}
