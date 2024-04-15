using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseValidatorAft : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    private readonly TestCase _expectedResult;
    private readonly TestGroup _group;
    private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, MLKEMDecapsulationResult> _deferredResolver;
    private readonly KyberParameters _kyberParameters;
    
    public int TestCaseId => _expectedResult.TestCaseId;

    public TestCaseValidatorAft(TestCase expectedResult, TestGroup group, IDeferredTestCaseResolverAsync<TestGroup, TestCase, MLKEMDecapsulationResult> deferredResolver)
    {
        _expectedResult = expectedResult;
        _group = group;
        _deferredResolver = deferredResolver;
        _kyberParameters = new KyberParameters(group.ParameterSet);
    }
    
    public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
    {
        var errors = new List<string>();
        var expected = new Dictionary<string, string>();
        var provided = new Dictionary<string, string>();

        ValidateResultPresent(suppliedResult, errors);
        if (errors.Count == 0)
        {
            ValidateResultLengths(suppliedResult, errors);
        }

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
                Reason = string.Join("; ", errors),
                Expected = expected.Count != 0 && showExpected ? expected : null,
                Provided = provided.Count != 0 && showExpected ? provided : null
            };
        }

        return new TestCaseValidation
        {
            TestCaseId = TestCaseId,
            Result = Core.Enums.Disposition.Passed
        };
    }

    private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
    {
        if (suppliedResult.Ciphertext == null)
        {
            errors.Add($"{nameof(suppliedResult.Ciphertext)} was not present in the {nameof(TestCase)}");
        }
    }

    private void ValidateResultLengths(TestCase suppliedResult, List<string> errors)
    {
        // This should never be a non-module 8 value
        if (suppliedResult.Ciphertext.BitLength % 8 != 0)
        {
            errors.Add("Ciphertext was not provided as complete bytes");
            return;
        }

        var ctByteLength = suppliedResult.Ciphertext.BitLength / 8;
        if (ctByteLength != _kyberParameters.CiphertextLength)
        {
            errors.Add($"Ciphertext not the expected length, provided: {ctByteLength} bytes, expected: {_kyberParameters.CiphertextLength} bytes");
            return;
        }
    }

    private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        var verifyResult = await _deferredResolver.CompleteDeferredCryptoAsync(_group, _expectedResult, suppliedResult);
        if (verifyResult.ImplicitRejection)
        {
            errors.Add($"Validation failed: implicit rejection detected");
        }
        else
        {
            if (!_expectedResult.SharedKey.Equals(suppliedResult.SharedKey))
            {
                errors.Add($"{nameof(suppliedResult.SharedKey)} does not match expected valid shared key");
                expected.Add(nameof(_expectedResult.SharedKey), _expectedResult.SharedKey.ToHex());
                provided.Add(nameof(suppliedResult.SharedKey), suppliedResult.SharedKey.ToHex());
            }
        }
    }
}
