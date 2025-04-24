using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.CXOF128;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.CXOF128.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.Ascon.SP800_232.CXOF.ContractResolvers;

[TestFixture, UnitTest, FastIntegrationTest]
public class PromptProjectionContractResolverTests
{
    private readonly JsonConverterProvider _jsonConverterProvider = new JsonConverterProvider();
    private readonly ContractResolverFactory _contractResolverFactory = new ContractResolverFactory();
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

    /// <summary>
    /// All group level properties are present in the prompt file
    /// </summary>
    [Test]
    public void ShouldSerializeGroupProperties()
    {
        var tvs = TestDataMother.GetTestGroups();
        var tg = tvs.TestGroups[0];

        var json = _serializer.Serialize(tvs, _projection);
        var newTvs = _deserializer.Deserialize(json);

        var newTg = newTvs.TestGroups[0];

        Assert.Multiple(() =>
        {
            Assert.That(tg.TestGroupId, Is.EqualTo(newTg.TestGroupId));
            Assert.That(tg.Tests, Has.Count.EqualTo(newTg.Tests.Count));
        });
    }

    /// <summary>
    /// Include everything but the answer (returnedBits)
    /// </summary>
    [Test]
    public void ShouldSerializeCaseProperties()
    {
        var tvs = TestDataMother.GetTestGroups();
        var tg = tvs.TestGroups[0];
        var tc = tg.Tests[0];

        var json = _serializer.Serialize(tvs, _projection);
        var newTvs = _deserializer.Deserialize(json);

        var newTg = newTvs.TestGroups[0];
        var newTc = newTg.Tests[0];

        Assert.Multiple(() =>
        {
            Assert.That(tc.ParentGroup.TestGroupId, Is.EqualTo(newTg.TestGroupId));
            Assert.That(tc.TestCaseId, Is.EqualTo(newTc.TestCaseId));
            Assert.That(tc.Message, Is.EqualTo(newTc.Message));
            Assert.That(tc.MessageBitLength, Is.EqualTo(newTc.MessageBitLength));
            Assert.That(tc.DigestBitLength, Is.EqualTo(newTc.DigestBitLength));
            Assert.That(tc.CSBitLength, Is.EqualTo(newTc.CSBitLength));
            Assert.That(tc.CS, Is.EqualTo(newTc.CS));
            Assert.That(tc.Digest, Is.Not.EqualTo(newTc.Digest));
        });

        // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
        Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
        Assert.That(regex.Matches(json), Is.Empty);
    }
}
