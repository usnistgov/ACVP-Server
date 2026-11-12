using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_KEM.FIPS203.v1_0.KeyGen;

[TestFixture, UnitTest]
public class ParameterValidatorTests
{
    private readonly ParameterValidator _subject = new();

    [Test]
    public void ValidationShouldFailWhenParameterSetsIsMissing()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-KEM",
            Mode = "keyGen",
            Revision = "FIPS203"
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void ValidationShouldFailWhenParameterSetsIsEmpty()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-KEM",
            Mode = "keyGen",
            Revision = "FIPS203",
            ParameterSets = Array.Empty<MLKEMParameterSet>()
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void ValidationShouldFailForInvalidParameterSetsValue()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-KEM",
            Mode = "keyGen",
            Revision = "FIPS203",
            ParameterSets = new[] { (MLKEMParameterSet)999 }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void ValidationShouldSucceedForValidParameterSet()
    {
        var parameters = new Parameters()
        {
            Algorithm = "ML-KEM",
            Mode = "keyGen",
            Revision = "FIPS203",
            ParameterSets = new[] { MLKEMParameterSet.ML_KEM_768 }
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.True);
    }
}
