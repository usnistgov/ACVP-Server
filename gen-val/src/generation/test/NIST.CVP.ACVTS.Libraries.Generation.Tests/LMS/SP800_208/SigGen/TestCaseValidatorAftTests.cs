using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.SP800_208.SigGen;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SP800_208.SigGen;

[TestFixture, UnitTest]
public class TestCaseValidatorAftTests
{
    private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, LmsVerificationResult>> _mockResolver;
    private TestGroup _group;
    private TestCase _expectedResult;

    [SetUp]
    public void Setup()
    {
        _mockResolver = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, LmsVerificationResult>>();
        _group = new TestGroup { TestGroupId = 1 };
        _expectedResult = new TestCase { TestCaseId = 1, Signature = new BitString("0123456789ABCDEF") };
    }

    [Test]
    public async Task ValidateAsync_MissingSignature_ReturnsFailed()
    {
        // Arrange
        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);
        var supplied = new TestCase { TestCaseId = 1, Signature = null, ParentGroup = _group};

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain("Could not find signature"));
    }

    [Test]
    public async Task ValidateAsync_VerificationFails_ReturnsFailed()
    {
        // Arrange
        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD"), ParentGroup = _group };

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult("Invalid Signature"));

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain("Validation failed: Invalid Signature"));
    }

    [Test]
    public async Task ValidateAsync_VerificationSucceeds_ReturnsPassed()
    {
        // Arrange
        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD"), ParentGroup = _group };

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
    }
    
    [Test]
    public void ValidateAsync_DuplicatePrivateKey_ReturnsFailed()
    {
        // Arrange
        var suppliedSignature = new BitString("AABBCCDD11223344");
        var duplicateSignature = new BitString("AABBCCDD55667788"); // First 4 bytes (AABBCCDD) are identical
        
        // Insert the duplicate test case into the parent group
        var supplied = new TestCase { TestCaseId = 1, Signature = suppliedSignature };
        supplied.ParentGroup = new TestGroup
        {
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = duplicateSignature } }
        };
        
        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult());

        // Act
        var result = validator.ValidateAsync(supplied).Result;

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain("Duplicate private key detected"));
        Assert.That(result.Reason, Does.Contain("tcId: 1"));
        Assert.That(result.Reason, Does.Contain("tcId: 2"));
    }

    [Test]
    public async Task ValidateAsync_UniquePrivateKey_ReturnsPassed()
    {
        // Arrange
        var suppliedSignature = new BitString("AABBCCDD11223344");
        var uniqueSignature = new BitString("FFEECCDD55667788"); // Different prefix

        // Insert the duplicate test case into the parent group
        var supplied = new TestCase { TestCaseId = 1, Signature = suppliedSignature };
        supplied.ParentGroup = new TestGroup
        {
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = uniqueSignature } }
        };
        
        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
    }

    [Test]
    [TestCase(null, -1)]
    [TestCase("", -1)]
    [TestCase("AABBCC", -1)]
    [TestCase("AABBCCDD", 31)]
    public async Task ValidateAsync_OtherTestCaseSignatureMissingOrTooShort_ReturnsPassed(string otherSignatureHex, int bitLength)
    {
        // Arrange
        var otherSignature = otherSignatureHex == null ? null : new BitString(otherSignatureHex, bitLength);

        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD11223344") };
        supplied.ParentGroup = new TestGroup
        {
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = otherSignature } }
        };

        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
    }

    [Test]
    public async Task ValidateAsync_OtherTestCaseSignatureOf32BitsWithSameQ_ReturnsFailed()
    {
        // Arrange
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD11223344") };
        supplied.ParentGroup = new TestGroup
        {
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = new BitString("AABBCCDD") } }
        };

        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain("Duplicate private key detected for tcId: 1 and tcId: 2"));
    }

    [Test]
    [TestCase("", -1)]
    [TestCase("AABBCC", -1)]
    [TestCase("AABBCCDD", 31)]
    public async Task ValidateAsync_SignatureTooShort_ReturnsFailed(string signatureHex, int bitLength)
    {
        // Arrange
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString(signatureHex, bitLength) };
        supplied.ParentGroup = new TestGroup
        {
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = new BitString("FFEECCDD55667788") } }
        };

        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain("Signature is too short to contain q"));
    }

    [Test]
    public async Task ValidateAsync_DuplicatePrivateKeyWithMalformedTestCaseInGroup_ReturnsFailed()
    {
        // Arrange
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD11223344") };
        supplied.ParentGroup = new TestGroup
        {
            Tests =
            {
                supplied,
                new TestCase { TestCaseId = 2, Signature = null },
                new TestCase { TestCaseId = 3, Signature = new BitString("AABBCC") },
                new TestCase { TestCaseId = 4, Signature = new BitString("AABBCCDD55667788") } // First 4 bytes (AABBCCDD) are identical
            }
        };

        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new LmsVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain("Duplicate private key detected for tcId: 1 and tcId: 4"));
        Assert.That(result.Reason, Does.Not.Contain("tcId: 2"));
        Assert.That(result.Reason, Does.Not.Contain("tcId: 3"));
    }
}
