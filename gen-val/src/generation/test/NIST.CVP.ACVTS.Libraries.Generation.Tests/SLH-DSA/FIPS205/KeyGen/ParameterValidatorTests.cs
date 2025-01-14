using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.KeyGen;

[TestFixture, UnitTest]
public class ParameterValidatorTests
{
    private readonly ParameterValidator _subject = new();

    private class ParameterBuilder
    {
        private string _algo = "SLH-DSA";
        private string _mode = "keyGen";
        private string _revision = "FIPS205";
        private SlhdsaParameterSet[] _parameterSets = new SlhdsaParameterSet[] { SlhdsaParameterSet.SLH_DSA_SHA2_128f };

        public ParameterBuilder()
        {
            //_parameterSets = 
        }
        
        public ParameterBuilder WithAlgoModeRevision(string algo, string mode, string revision)
        {
            _algo = algo;
            _mode = mode;
            _revision = revision;
            return this;
        }
        
        public ParameterBuilder WithParameterSets(SlhdsaParameterSet[] value)
        {
            _parameterSets = value;
            return this;
        }
        
        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algo,
                Mode = _mode,
                Revision = _revision,
                ParameterSets = _parameterSets
            };
        }
    }

    [Test] // ISSUE #3
    public void ValidationShouldFailWhenParameterSetsIsMissing()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "keyGen",
            Revision = "FIPS205"
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    [Test] // ISSUE #2
    public void ValidationShouldFailWhenParameterSetsIsEmpty()
    {
        var parameters = new Parameters()
        {
            Algorithm = "SLH-DSA",
            Mode = "keyGen",
            Revision = "FIPS205",
            ParameterSets = System.Array.Empty<SlhdsaParameterSet>()
        };

        var result = _subject.Validate(parameters);

        Assert.That(result.Success, Is.False);
    }
    
    // I'm not really sure how to test this...
    // [Test] // ISSUE #1
    // public void ValidationShouldFailForInvalidParameterSetsValue()
    // {
    //     var parameters = new Parameters()
    //     {
    //         Algorithm = "SLH-DSA",
    //         Mode = "keyGen",
    //         Revision = "FIPS205",
    //         ParameterSets = new SlhdsaParameterSet[] { SlhdsaParameterSet.Invalid }
    //     };
    //
    //     var result = _subject.Validate(parameters);
    //
    //     Assert.IsFalse(result.Success);
    // }
    
}
