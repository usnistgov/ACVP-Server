using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.SigGen;

[TestFixture, UnitTest]
public class ParameterValidatorTests
{
    private readonly ParameterValidator _subject = new();
    
    [Test] 
    public void ValidationShouldSucceedWithValidRegistration()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true },
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 8))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.True);
    }
    
    [Test] 
    public void ValidationShouldFailWhenDeterministicIsMissing()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 8))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWhenDeterministicIsEmpty()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 8))
                }
            },
            Deterministic = System.Array.Empty<bool>()
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWhenCapabilitiesIsMissing()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWhenCapabilitiesIsEmpty()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true },
            Capabilities = System.Array.Empty<Capability>()
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWithRepeatsInDeterministic()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true, false, true },
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 8))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWithNoParameterSetsInCapability()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true, false },
            Capabilities = new []
            {
                new Capability()
                {
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 8))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWithNoMessageLengthInCapability()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true, false },
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f }
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWithEmptyParameterSetsInCapability()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true, false },
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = System.Array.Empty<SlhdsaParameterSet>(),
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 8))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWithParameterSetsRepeatsInCapability()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true, false },
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f, SlhdsaParameterSet.SLH_DSA_SHA2_128f, SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 8))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWithNonMultipleOfEightMessageLengthInCapability()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true, false },
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(15))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] 
    public void ValidationShouldFailWithOutOfRangeMessageLengthsInCapability()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "sigGen",
            Revision = "FIPS205",
            Deterministic = new [] { true, false },
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(65544))
                }
            }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
}
