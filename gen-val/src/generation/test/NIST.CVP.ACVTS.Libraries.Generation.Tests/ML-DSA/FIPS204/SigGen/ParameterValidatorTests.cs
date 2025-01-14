using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.SigGen;

[TestFixture, UnitTest]
public class ParameterValidatorTests
{
    private ParameterValidator _subject;
    
    [SetUp]
    public void SetUp()
    {
        _subject = new ParameterValidator();
    }
    
    [Test]
    public void WhenGivenDefaultParameterBuilder_ShouldPass()
    {
        var pb = new ParameterBuilder();
        var p = pb.Build();

        var result = _subject.Validate(p);

        Assert.That(result.Success, Is.True);
    }

    private static readonly object[] SpecificCapabilities =
    {
        new object[]
        {
            "valid params",
            new [] { SignatureInterface.Internal, SignatureInterface.External },
            new [] { PreHash.PreHash, PreHash.Pure },
            new [] { HashFunctions.Sha2_d256 },
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            null,
            true
        },
        new object[]
        {
            "invalid signature interface",
            new [] { SignatureInterface.Internal, SignatureInterface.None },
            new [] { PreHash.PreHash, PreHash.Pure },
            new [] { HashFunctions.Sha2_d256 },
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            null,
            false
        },
        new object[]
        {
            "empty signature interface",
            Array.Empty<SignatureInterface>(),
            new [] { PreHash.PreHash, PreHash.Pure },
            new [] { HashFunctions.Sha2_d256 },
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            null,
            false
        },
        new object[]
        {
            "invalid prehash",
            new [] { SignatureInterface.Internal, SignatureInterface.External },
            new [] { PreHash.PreHash, PreHash.None },
            new [] { HashFunctions.Sha2_d256 },
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            null,
            false
        },
        new object[]
        {
            "empty prehash but including external",
            new [] { SignatureInterface.Internal, SignatureInterface.External },
            Array.Empty<PreHash>(),
            new [] { HashFunctions.Sha2_d256 },
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            null,
            false
        },
        new object[]
        {
            "invalid context length - too long",
            new [] { SignatureInterface.Internal, SignatureInterface.External },
            new [] { PreHash.PreHash, PreHash.Pure },
            new [] { HashFunctions.Sha2_d256 },
            new MathDomain().AddSegment(new ValueDomainSegment(9000)),
            null,
            false
        },
        new object[]
        {
            "invalid context length - not multiple of 8",
            new [] { SignatureInterface.Internal, SignatureInterface.External },
            new [] { PreHash.PreHash, PreHash.Pure },
            new [] { HashFunctions.Sha2_d256 },
            new MathDomain().AddSegment(new ValueDomainSegment(1025)),
            null,
            false
        },
        new object[]
        {
            "invalid hashes",
            new [] { SignatureInterface.Internal, SignatureInterface.External },
            new [] { PreHash.PreHash, PreHash.Pure },
            new [] { HashFunctions.None },
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            null,
            false
        },
        new object[]
        {
            "internal but no prehash + context, with hash function",
            new [] { SignatureInterface.Internal },
            Array.Empty<PreHash>(),
            new [] { HashFunctions.Sha2_d256 },
            null,
            null,
            false
        },
        new object[]
        {
            "internal but no hash function + context, with prehash",
            new [] { SignatureInterface.Internal },
            new [] { PreHash.PreHash },
            Array.Empty<HashFunctions>(),
            null,
            null,
            false
        },
        new object[]
        {
            "internal but no prehash + hash function, with context",
            new [] { SignatureInterface.Internal },
            Array.Empty<PreHash>(),
            Array.Empty<HashFunctions>(),
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            null,
            false
        },
        new object[]
        {
            "internal",
            new [] { SignatureInterface.Internal },
            Array.Empty<PreHash>(),
            Array.Empty<HashFunctions>(),
            null, 
            null,
            true
        },
        new object[]
        {
            "external with prehash, no hash function",
            new [] { SignatureInterface.External },
            new [] { PreHash.PreHash },
            Array.Empty<HashFunctions>(),
            new MathDomain().AddSegment(new ValueDomainSegment(128)), 
            new [] { false },
            false
        },
        new object[]
        {
            "external with prehash, no context",
            new [] { SignatureInterface.External },
            new [] { PreHash.PreHash },
            new [] { HashFunctions.Sha3_d512 },
            null, 
            new [] { false },
            false
        },
        new object[]
        {
            "external with prehash",
            new [] { SignatureInterface.External },
            new [] { PreHash.PreHash },
            new [] { HashFunctions.Sha3_d512 },
            new MathDomain().AddSegment(new ValueDomainSegment(128)), 
            new [] { false },
            true
        },
        new object[]
        {
            "external with pure, hash function",
            new [] { SignatureInterface.External },
            new [] { PreHash.Pure },
            new [] { HashFunctions.Sha3_d512 },
            new MathDomain().AddSegment(new ValueDomainSegment(128)), 
            new [] { false },
            false
        },
        new object[]
        {
            "external with pure, no hash, with context",
            new [] { SignatureInterface.External },
            new [] { PreHash.Pure },
            Array.Empty<HashFunctions>(),
            new MathDomain().AddSegment(new ValueDomainSegment(128)), 
            new [] { false },
            true
        },
        new object[]
        {
            "external mu with external interface",
            new [] { SignatureInterface.External },
            new [] { PreHash.Pure },
            Array.Empty<HashFunctions>(),
            new MathDomain().AddSegment(new ValueDomainSegment(128)),
            new [] { true },
            false
        }
    };
    
    [Test]
    [TestCaseSource(nameof(SpecificCapabilities))]
    public void WhenGivenParameters_ShouldVerifyOnlyValidCombinations(string description, SignatureInterface[] signatureInterfaces, PreHash[] preHashes, HashFunctions[] hashFunctions, MathDomain contextLength, bool[] externalMu, bool expectedSuccess)
    {
        var p = new ParameterBuilder()
            .WithSignatureInterfaces(signatureInterfaces)
            .WithPreHash(preHashes)
            .WithHashAlgs(hashFunctions)
            .WithContextLength(contextLength)
            .WithExternalMu(externalMu)
            .Build();

        var result = _subject.Validate(p);

        Assert.That(expectedSuccess, Is.EqualTo(result.Success), result.ErrorMessage);
    }
}
