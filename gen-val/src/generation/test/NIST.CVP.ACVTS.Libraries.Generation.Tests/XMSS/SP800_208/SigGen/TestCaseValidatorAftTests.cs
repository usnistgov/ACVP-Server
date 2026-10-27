using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.SigGen;

[TestFixture]
public class TestCaseValidatorAftTests
{
    private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, XmssVerificationResult>> _mockResolver;
    private TestGroup _group;
    private TestCase _expectedResult;

    // XMSS-SHA2_10_256 OID followed by a 32-byte root and a 32-byte SEED
    private static readonly string PublicKeyHex = "00000001" + new string('A', 128);

    [SetUp]
    public void Setup()
    {
        _mockResolver = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, XmssVerificationResult>>();
        _group = new TestGroup { TestGroupId = 1, XmssMode = XmssMode.XMSS_SHA2_10_256, PublicKey = new BitString(PublicKeyHex) };
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
            .ReturnsAsync(new XmssVerificationResult("Invalid Signature"));

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
            .ReturnsAsync(new XmssVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
    }

    [Test]
    [TestCase("0000000D")] // XMSS-SHA2_10_192
    [TestCase("00000099")] // not an XMSS type code
    public async Task ValidateAsync_PublicKeyOidMismatch_ReturnsFailed(string oidHex)
    {
        // Arrange
        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);
        var suppliedGroup = new TestGroup { PublicKey = new BitString(oidHex + new string('A', 128)) };
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD"), ParentGroup = suppliedGroup };

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain($"Public key OID {oidHex} does not match the XMSS mode XMSS-SHA2_10_256"));
        _mockResolver.Verify(r => r.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Never);
    }

    [Test]
    [TestCase(null, "Could not find public key")]
    [TestCase("0000", "Public key provided was not the correct size")]
    public async Task ValidateAsync_MissingOrShortPublicKey_ReturnsFailed(string publicKeyHex, string expectedReason)
    {
        // Arrange
        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);
        var suppliedGroup = new TestGroup { PublicKey = publicKeyHex == null ? null : new BitString(publicKeyHex) };
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD"), ParentGroup = suppliedGroup };

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain(expectedReason));
        _mockResolver.Verify(r => r.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Never);
    }

    [Test]
    public void ValidateAsync_DuplicatePrivateKey_ReturnsFailed()
    {
        // Arrange
        var suppliedSignature = new BitString("AABBCCDD11223344");
        var duplicateSignature = new BitString("AABBCCDD55667788"); // First 4 bytes (idx_sig, AABBCCDD) are identical

        // Insert the duplicate test case into the parent group
        var supplied = new TestCase { TestCaseId = 1, Signature = suppliedSignature };
        supplied.ParentGroup = new TestGroup
        {
            PublicKey = new BitString(PublicKeyHex),
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = duplicateSignature } }
        };

        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new XmssVerificationResult());

        // Act
        var result = validator.ValidateAsync(supplied).Result;

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        Assert.That(result.Reason, Does.Contain("Duplicate private key detected"));
        Assert.That(result.Reason, Does.Contain("tcId: 1"));
        Assert.That(result.Reason, Does.Contain("tcId: 2"));
    }

    [Test]
    [TestCase(null)]
    [TestCase("AABB")]
    public async Task ValidateAsync_SiblingSignatureMissingOrShort_ReturnsPassed(string siblingSignatureHex)
    {
        // Arrange
        var supplied = new TestCase { TestCaseId = 1, Signature = new BitString("AABBCCDD11223344") };
        supplied.ParentGroup = new TestGroup
        {
            PublicKey = new BitString(PublicKeyHex),
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = siblingSignatureHex == null ? null : new BitString(siblingSignatureHex) } }
        };

        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new XmssVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
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
            PublicKey = new BitString(PublicKeyHex),
            Tests = { supplied, new TestCase { TestCaseId = 2, Signature = uniqueSignature } }
        };

        var validator = new TestCaseValidatorAft(_expectedResult, _group, _mockResolver.Object);

        _mockResolver.Setup(r => r.CompleteDeferredCryptoAsync(_group, _expectedResult, supplied))
            .ReturnsAsync(new XmssVerificationResult());

        // Act
        var result = await validator.ValidateAsync(supplied);

        // Assert
        Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
    }
}
