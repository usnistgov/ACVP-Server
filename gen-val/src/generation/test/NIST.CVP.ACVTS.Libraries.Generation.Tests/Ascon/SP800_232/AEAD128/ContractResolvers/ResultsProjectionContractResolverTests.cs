using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.Ascon.SP800_232.AEAD128.ContractResolvers;

[TestFixture, UnitTest, FastIntegrationTest]
public class ResultsProjectionContractResolverTests
{
    private readonly JsonConverterProvider _jsonConverterProvider = new JsonConverterProvider();
    private readonly ContractResolverFactory _contractResolverFactory = new ContractResolverFactory();
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

    /// <summary>
    /// Only the groupId and tests should be present in the result file
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
            Assert.That(newTg.Tests, Has.Count.EqualTo(tg.Tests.Count), nameof(newTg.Tests.Count));
            Assert.That(tg.Direction, Is.Not.EqualTo(newTg.Direction), nameof(newTg.Direction));
            Assert.That(tg.PlaintextLength, Is.Not.EqualTo(newTg.PlaintextLength), nameof(newTg.PlaintextLength));
            Assert.That(tg.ADLength, Is.Not.EqualTo(newTg.ADLength), nameof(newTg.ADLength));
            Assert.That(tg.TruncationLength, Is.Not.EqualTo(newTg.TruncationLength), nameof(newTg.TruncationLength));
            Assert.That(tg.NonceMasking, Is.Not.EqualTo(newTg.NonceMasking), nameof(newTg.NonceMasking));
        });
    }

    /// <summary>
    /// Only includes the test case Id and the returnedBits
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
        });

        if (tc.ParentGroup.Direction == BlockCipherDirections.Encrypt)
        {
            Assert.Multiple(() =>
            {
                Assert.That(tc.Ciphertext, Is.EqualTo(newTc.Ciphertext));
                Assert.That(tc.Tag, Is.EqualTo(newTc.Tag));
                Assert.That(tc.Plaintext, Is.Not.EqualTo(newTc.Plaintext));
            });
        }
        else
        {
            Assert.Multiple(() =>
            {
                Assert.That(tc.Plaintext, Is.EqualTo(newTc.Plaintext));
                Assert.That(tc.Tag, Is.Not.EqualTo(newTc.Tag));
                Assert.That(tc.Ciphertext, Is.Not.EqualTo(newTc.Ciphertext));
            });
        }

        Assert.Multiple(() =>
        {
            Assert.That(tc.Key, Is.Not.EqualTo(newTc.Key));
            Assert.That(tc.Nonce, Is.Not.EqualTo(newTc.Nonce));
            Assert.That(tc.AD, Is.Not.EqualTo(newTc.AD));
            Assert.That(tc.PayloadBitLength, Is.Not.EqualTo(newTc.PayloadBitLength));
            Assert.That(tc.ADBitLength, Is.Not.EqualTo(newTc.ADBitLength));
            Assert.That(tc.TagLength, Is.Not.EqualTo(newTc.TagLength));
            Assert.That(tc.SecondKey, Is.Not.EqualTo(newTc.SecondKey));
        });

        // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
        Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
        Assert.That(regex.Matches(json), Is.Empty);
    }
}
