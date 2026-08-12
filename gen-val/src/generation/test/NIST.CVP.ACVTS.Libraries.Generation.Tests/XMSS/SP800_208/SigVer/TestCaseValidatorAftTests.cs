using System.Threading.Tasks;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.SigVer;

[TestFixture]
public class TestCaseValidatorAftTests
{
    [Test]
    public async Task ValidateAsync_MatchingResult_ReturnsPassed()
    {
        // Arrange
        var expected = new TestCase { TestCaseId = 1, TestPassed = true };
        var supplied = new TestCase { TestCaseId = 1, TestPassed = true };
        var validator = new TestCaseValidatorAft(expected);

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(NIST.CVP.ACVTS.Libraries.Generation.Core.Enums.Disposition.Passed));
    }

    [Test]
    public async Task ValidateAsync_MismatchedResult_ReturnsFailed()
    {
        // Arrange
        var expected = new TestCase { TestCaseId = 1, TestPassed = true, Reason = XmssSignatureDisposition.ModifySignature };
        var supplied = new TestCase { TestCaseId = 1, TestPassed = false };
        var validator = new TestCaseValidatorAft(expected);

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(NIST.CVP.ACVTS.Libraries.Generation.Core.Enums.Disposition.Failed));
        Assert.That(result.Expected, Is.Not.Null);
        Assert.That(result.Provided, Is.Not.Null);
    }
}
