using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer.ContractResolvers;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.SigVer;

public class PromptProjectionContractResolverTests
{
    private readonly JsonConverterProvider _jsonConverterProvider = new();
    private readonly ContractResolverFactory _contractResolverFactory = new();
    private readonly Projection _projection = Projection.Prompt;

    private VectorSetSerializer<TestVectorSet, TestGroup, TestCase> _serializer;
    private VectorSetDeserializer<TestVectorSet, TestGroup, TestCase> _deserializer;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _serializer =
            new VectorSetSerializer<TestVectorSet, TestGroup, TestCase>(
                _jsonConverterProvider,
                _contractResolverFactory
            );
        _deserializer =
            new VectorSetDeserializer<TestVectorSet, TestGroup, TestCase>(
                _jsonConverterProvider
            );
    }
    
    [Test]
    public void ShouldSerializeGroupProperties()
    {
        var tvs = TestDataMother.GetTestGroups();
        var tg = tvs.TestGroups[0];

        var json = _serializer.Serialize(tvs, _projection);
        var newTvs = _deserializer.Deserialize(json);

        var newTg = newTvs.TestGroups[0];

        Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
        Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
        Assert.That(newTg.ParameterSet, Is.EqualTo(tg.ParameterSet), nameof(newTg.ParameterSet));
        Assert.That(newTg.MessageLengths, Is.Not.EqualTo(tg.MessageLengths), nameof(newTg.MessageLengths));
        Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
    }
    
    [Test]
    public void ShouldSerializeCaseProperties()
    {
        var tvs = TestDataMother.GetTestGroups(2);
        var tg = tvs.TestGroups[0];
        var tc = tg.Tests[0];

        var json = _serializer.Serialize(tvs, _projection);
        var newTvs = _deserializer.Deserialize(json);

        var newTg = newTvs.TestGroups[0];
        var newTc = newTg.Tests[0];

        Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));

        // Prompt properties
        Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
        Assert.That(newTc.PublicKey, Is.EqualTo(tc.PublicKey), nameof(newTc.PublicKey));
        Assert.That(newTc.MessageLength, Is.EqualTo(tc.MessageLength), nameof(newTc.MessageLength));
        Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
        Assert.That(newTc.Signature, Is.EqualTo(tc.Signature), nameof(newTc.Signature));

        // Response properties
        Assert.That(newTc.Reason, Is.Not.EqualTo(tc.Reason), nameof(newTc.Reason));

        // Internal Projection properties
        Assert.That(newTc.PrivateKey, Is.Not.EqualTo(tc.PrivateKey), nameof(newTc.PrivateKey));
        Assert.That(newTc.AdditionalRandomness, Is.Not.EqualTo(tc.AdditionalRandomness), nameof(newTc.AdditionalRandomness));
        
        // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
        var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
        Assert.That(regex.Matches(json).Count == 0, Is.True);
    }
    
}
