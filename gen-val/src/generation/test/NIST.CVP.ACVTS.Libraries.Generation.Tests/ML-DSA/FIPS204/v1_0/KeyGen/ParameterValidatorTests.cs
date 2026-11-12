using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.v1_0.KeyGen;

[TestFixture, UnitTest]
public class ParameterValidatorTests
{
    private readonly ParameterValidator _subject = new();

    [Test]
    public void ValidationShouldFailWhenParameterSetsIsMissing()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-DSA",
            Mode = "keyGen",
            Revision = "FIPS204"
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void ValidationShouldFailWhenParameterSetsIsEmpty()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-DSA",
            Mode = "keyGen",
            Revision = "FIPS204",
            ParameterSets = Array.Empty<DilithiumParameterSet>()
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void ValidationShouldFailForInvalidParameterSetsValue()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-DSA",
            Mode = "keyGen",
            Revision = "FIPS204",
            ParameterSets = new[] { (DilithiumParameterSet)999 }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void ValidationShouldSucceedForValidParameterSet()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-DSA",
            Mode = "keyGen",
            Revision = "FIPS204",
            ParameterSets = new[] { DilithiumParameterSet.ML_DSA_65 }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.True);
    }
}
