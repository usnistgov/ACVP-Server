using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.KeyGen;

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
    public async Task ShouldFailIfKeysAreNotPresent()
    {
        var testCase = GetTestCase();
        _subject = new TestCaseValidator(testCase);
        var suppliedResult = new TestCase
        {
            TestCaseId = 1
        };

        var result = await _subject.ValidateAsync(suppliedResult);
        Assert.That(result != null);
        Assert.That(Core.Enums.Disposition.Failed == result.Result);

        Assert.That(result.Reason.Contains($"{nameof(suppliedResult.PublicKey)} was not present in the {nameof(TestCase)}"), Is.True);
        Assert.That(result.Reason.Contains($"{nameof(suppliedResult.PrivateKey)} was not present in the {nameof(TestCase)}"), Is.True);
    }

    [Test]
    public async Task ShouldShowPubPrivKeysAsReasonIfTheyDoNotMatch()
    {
        var testPublicKey = new BitString("000000000000000000000000000000");
        var testPrivateKey = new BitString("111111111111111111111111111111111111111111111111111111111111");
        
        var testCase = GetTestCase();
        _subject = new TestCaseValidator(testCase);

        var suppliedResult = GetTestCase();
        suppliedResult.PublicKey = testPublicKey;
        suppliedResult.PrivateKey = testPrivateKey;

        var result = await _subject.ValidateAsync(suppliedResult);
        Assert.That(result != null);
        Assert.That(Core.Enums.Disposition.Failed == result.Result);
        Assert.That(result.Reason.Contains($"{nameof(suppliedResult.PublicKey)} does not match"), Is.True);
        Assert.That(result.Reason.Contains($"{nameof(suppliedResult.PrivateKey)} does not match"), Is.True);
    }

    private TestCase GetTestCase()
    {
        var testCase = new TestCase
        {
            TestCaseId = 1,
            PublicKey = new BitString("ABCDEF0123456789ABCDEF0123456789"),
            PrivateKey = new BitString("ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789")
        };
         
        return testCase;
    }
}
