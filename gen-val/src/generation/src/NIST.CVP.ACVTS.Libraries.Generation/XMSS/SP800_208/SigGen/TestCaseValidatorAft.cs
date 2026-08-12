using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;

public class TestCaseValidatorAft : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    private readonly TestCase _expectedResult;
    private readonly TestGroup _group;
    private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, XmssVerificationResult> _deferredResolver;

    public int TestCaseId => _expectedResult.TestCaseId;

    public TestCaseValidatorAft(TestCase expectedResult, TestGroup group, IDeferredTestCaseResolverAsync<TestGroup, TestCase, XmssVerificationResult> deferredResolver)
    {
        _expectedResult = expectedResult;
        _group = group;
        _deferredResolver = deferredResolver;
    }

    public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
    {
        var errors = new List<string>();
        Dictionary<string, string> expected = new Dictionary<string, string>();
        Dictionary<string, string> provided = new Dictionary<string, string>();

        ValidateResultPresent(suppliedResult, errors);
        if (errors.Count == 0)
        {
            ValidatePrivateKeyUniqueness(suppliedResult, errors);
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

    private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
    {
        if (suppliedResult.Signature == null)
        {
            errors.Add("Could not find signature");
        }
    }

    private void ValidatePrivateKeyUniqueness(TestCase suppliedResult, List<string> errors)
    {
        foreach (var testCase in suppliedResult.ParentGroup.Tests)
        {
            if (suppliedResult.TestCaseId == testCase.TestCaseId)
            {
                continue;
            }

            if (suppliedResult.Signature.GetMostSignificantBits(32)
                .Equals(testCase.Signature.GetMostSignificantBits(32)))
            {
                errors.Add($"Duplicate private key detected for tcId: {suppliedResult.TestCaseId} and tcId: {testCase.TestCaseId}");
            }
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
