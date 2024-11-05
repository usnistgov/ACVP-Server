using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.SigVer;

[TestFixture, UnitTest]
public class TestCaseValidatorTests
{
    private TestCaseValidator _subject;

    [Test]
    public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
    {
        var testCase = GetTestCase();
        _subject = new TestCaseValidator(testCase);
        var result = await _subject.ValidateAsync(testCase);
        Assert.That(result != null);
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
    }
    
    [Test]
    public async Task ShouldFailTestsPassedIsNotPresent()
    {
        var testCase = GetTestCase();
        _subject = new TestCaseValidator(testCase);
        var suppliedResult = new TestCase
        {
            TestCaseId = 1
        };

        var result = await _subject.ValidateAsync(suppliedResult);
        Assert.That(result != null);
        Assert.That(result.Result == Core.Enums.Disposition.Failed);
        Assert.That(result.Reason.Contains($"Could not find TestPassed"), Is.True);
    }
    
    [Test]
    public async Task ShouldShowSignatureAsReasonIfTheyDoNotMatch()
    {
        var testCase = GetTestCase();
        testCase.TestPassed = false;
        testCase.Reason = SLHDSASignatureDisposition.ModifySignatureR;
        _subject = new TestCaseValidator(testCase);

        var suppliedResult = GetTestCase();

        var result = await _subject.ValidateAsync(suppliedResult);
        Assert.That(result != null);
        Assert.That(result.Result == Core.Enums.Disposition.Failed);
        Assert.That(result.Reason.Contains("modified signature - R modified"), Is.True);
    }
    
    private TestCase GetTestCase()
    {
        var testCase = new TestCase
        {
            TestCaseId = 1,
            TestPassed = true,
        };
         
        return testCase;
    }  
}
