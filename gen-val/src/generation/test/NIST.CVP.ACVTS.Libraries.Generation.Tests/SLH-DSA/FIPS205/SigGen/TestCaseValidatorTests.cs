using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.SigGen;

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
    public async Task ShouldFailIfSignatureIsNotPresent()
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
        Assert.That(result.Reason.Contains("Could not find signature"), Is.True);
    }
    
    [Test]
    public async Task ShouldShowSignatureAsReasonIfTheyDoNotMatch()
    {
        var testSignature = new BitString("00000000000000000000000000000000");
        
        var testCase = GetTestCase();
        _subject = new TestCaseValidator(testCase);

        var suppliedResult = GetTestCase();
        suppliedResult.Signature = testSignature;

        var result = await _subject.ValidateAsync(suppliedResult);
        Assert.That(result != null);
        Assert.That(result.Result == Core.Enums.Disposition.Failed);
        Assert.That(result.Reason.Contains("Incorrect signature"), Is.True);
    }
    
    private TestCase GetTestCase()
    {
        var testCase = new TestCase
        {
            TestCaseId = 1,
            Signature = new BitString("ABCDEF0123456789ABCDEF0123456789")
        };
         
        return testCase;
    }
}
