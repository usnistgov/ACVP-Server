using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.SigGen;

[TestFixture, UnitTest]
public class TestGroupGeneratorPoolsTests
{
    private TestGroupGeneratorPools _subject;

    private static object[] _generatorTestCases =
    [
        new object[] {
            "0 groups - externalMu does not contain false",
            new Parameters
            {
                ExternalMu = [true],
                SignatureInterfaces = [SignatureInterface.Internal],
                Deterministic = [true],
                Capabilities = [new Capability { MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(256)), ParameterSets = [DilithiumParameterSet.ML_DSA_44] }],
            },
            0
        },
        new object[] {
            "0 groups - signatureInterfaces does not contain internal",
            new Parameters
            {
                ExternalMu = [false],
                SignatureInterfaces = [SignatureInterface.External],
                Deterministic = [true],
                Capabilities = [new Capability { MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(256)), ParameterSets = [DilithiumParameterSet.ML_DSA_44] }]
            },
            0
        },
        new object[] {
            "0 groups - deterministic does not contain true",
            new Parameters
            {
                ExternalMu = [false],
                SignatureInterfaces = [SignatureInterface.Internal],
                Deterministic = [false],
                Capabilities = [new Capability { MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(256)), ParameterSets = [DilithiumParameterSet.ML_DSA_44] }]
            },
            0
        },
        new object[] {
            "0 groups - messageLength does not contain 256",
            new Parameters
            {
                ExternalMu = [false],
                SignatureInterfaces = [SignatureInterface.Internal],
                Deterministic = [true],
                Capabilities = [new Capability { MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(9000)), ParameterSets = [DilithiumParameterSet.ML_DSA_44] }]
            },
            0
        },
        new object[] {
            "2 groups - all properties exactly",
            new Parameters
            {
                ExternalMu = [false],
                SignatureInterfaces = [SignatureInterface.Internal],
                Deterministic = [true],
                Capabilities = [new Capability { MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(256)), ParameterSets = [DilithiumParameterSet.ML_DSA_44] }]
            },
            2
        },
        new object[] {
            "6 groups - all properties with everything",
            new Parameters
            {
                ExternalMu = [false, true],
                SignatureInterfaces = [SignatureInterface.Internal, SignatureInterface.External],
                Deterministic = [true, false],
                Capabilities = [new Capability { MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, PqcParameterValidator.MinMsgLen, PqcParameterValidator.MaxMsgLen)), ParameterSets = [DilithiumParameterSet.ML_DSA_44, DilithiumParameterSet.ML_DSA_65, DilithiumParameterSet.ML_DSA_87] }]
            },
            6
        }
    ];
    
    [Test]
    [TestCaseSource(nameof(_generatorTestCases))]
    public async Task ShouldReturnOneTestGroupForEachPool(string label, Parameters param, int expectedResultCount)
    {
        _subject = new TestGroupGeneratorPools();
        var result = await _subject.BuildTestGroupsAsync(param);

        Assert.That(result, Has.Count.EqualTo(expectedResultCount));
    }
}
