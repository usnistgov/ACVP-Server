using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseValidatorDecapsulationVal : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    private readonly TestCase _expectedResult;
    public int TestCaseId => _expectedResult.TestCaseId;

    public TestCaseValidatorDecapsulationVal(TestCase expectedResult)
    {
        _expectedResult = expectedResult;
    }

    public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
    {
        var errors = new List<string>();
        var expected = new Dictionary<string, string>();
        var provided = new Dictionary<string, string>();

        ValidateResultPresent(suppliedResult, errors);
        if (errors.Count == 0)
        {
            CheckResults(suppliedResult, errors, expected, provided);
        }

        if (errors.Count > 0)
        {
            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Failed,
                Reason = string.Join("; ", errors),
                Expected = expected.Count != 0 && showExpected ? expected : null,
                Provided = provided.Count != 0 && showExpected ? provided : null
            });
        }

        return Task.FromResult(new TestCaseValidation
        {
            TestCaseId = TestCaseId,
            Result = Core.Enums.Disposition.Passed
        });
    }

    private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
    {
        if (suppliedResult.SharedKey == null)
        {
            errors.Add($"{nameof(suppliedResult.SharedKey)} was not present in the {nameof(TestCase)}");
        }
    }

    private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        var errorForNoModification = "does not match expected valid shared key";
        var errorForModification = "does not match expected implicit rejection shared key";
        var error = EnumHelpers.GetEnumFromEnumDescription<MLKEMDecapsulationDisposition>(_expectedResult.Reason) == MLKEMDecapsulationDisposition.None ? errorForNoModification : errorForModification;
        
        // The expectedResult key is either the valid key or the implicit rejection key, both are pre-computed
        if (!_expectedResult.SharedKey.Equals(suppliedResult.SharedKey))
        {
            errors.Add($"{nameof(suppliedResult.SharedKey)} {error}");
            expected.Add(nameof(_expectedResult.SharedKey), _expectedResult.SharedKey.ToHex());
            provided.Add(nameof(suppliedResult.SharedKey), suppliedResult.SharedKey.ToHex());
        }
    }
}
