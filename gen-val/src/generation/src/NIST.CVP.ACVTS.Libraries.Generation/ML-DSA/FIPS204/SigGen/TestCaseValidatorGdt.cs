using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestCaseValidatorGdt : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    private readonly TestCase _expectedResult;
    private readonly TestGroup _group;
    private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, MLDSAVerificationResult> _deferredResolver;
    private readonly DilithiumParameters _dilithiumParameters;
    
    public int TestCaseId => _expectedResult.TestCaseId;

    public TestCaseValidatorGdt(TestCase expectedResult, TestGroup group, IDeferredTestCaseResolverAsync<TestGroup, TestCase, MLDSAVerificationResult> deferredResolver)
    {
        _expectedResult = expectedResult;
        _group = group;
        _deferredResolver = deferredResolver;
        _dilithiumParameters = new DilithiumParameters(group.ParameterSet);
    }

    public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
    {
        var errors = new List<string>();
        Dictionary<string, string> expected = new Dictionary<string, string>();
        Dictionary<string, string> provided = new Dictionary<string, string>();

        ValidateSignature(suppliedResult.Signature, errors);
        
        if (errors.Count == 0)
        {
            await CheckResults(suppliedResult, errors, expected, provided);
        }

        if (errors.Count > 0)
        {
            return new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Failed,
                Reason = string.Join(";", errors),
                Expected = showExpected ? expected : null,
                Provided = showExpected ? provided : null
            };
        }

        return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
    }
    
    private void ValidateSignature(BitString signature, List<string> errors)
    {
        if (signature == null)
        {
            errors.Add("Could not find signature");
            return;
        }
        
        // This should never be a non-modulo 8 value
        if (signature.BitLength % 8 != 0)
        {
            errors.Add("Signature not provided as complete bytes");
            return;
        }
        
        var sigByteLength = signature.BitLength / 8;
        if (sigByteLength != _dilithiumParameters.SignatureLength)
        {
            errors.Add($"Signature not the expected length, provided: {sigByteLength} bytes, expected: {_dilithiumParameters.SignatureLength} bytes");
            return;
        }
    }
    
    private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        var verifyResult = await _deferredResolver.CompleteDeferredCryptoAsync(_group, _expectedResult, suppliedResult);
        if (!verifyResult.Success)
        {
            errors.Add($"Validation failed: {verifyResult.ErrorMessage}");
        }
    }
}
