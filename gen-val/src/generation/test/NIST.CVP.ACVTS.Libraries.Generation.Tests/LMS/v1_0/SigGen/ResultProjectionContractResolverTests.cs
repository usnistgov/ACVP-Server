using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.v1_0.SigGen;

[TestFixture, UnitTest, FastIntegrationTest]
public class ResultsProjectionContractResolverTests
{
    private readonly JsonConverterProvider _jsonConverterProvider = new();
    private readonly ContractResolverFactory _contractResolverFactory = new();
    private readonly Projection _projection = Projection.Result;

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
        Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

        Assert.That(newTg.LmsMode, Is.Not.EqualTo(tg.LmsMode), nameof(newTg.LmsMode));
        Assert.That(newTg.LmOtsMode, Is.Not.EqualTo(tg.LmOtsMode), nameof(newTg.LmOtsMode));
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
        Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
        Assert.That(newTc.Signature, Is.EqualTo(tc.Signature), nameof(newTc.Signature));

        Assert.That(newTc.Message, Is.Not.EqualTo(tc.Message), nameof(newTc.Message));

        // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
        var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
        Assert.That(regex.Matches(json).Count == 0, Is.True);
    }
}
