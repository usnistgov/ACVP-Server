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
            Assert.That(tg.Direction, Is.EqualTo(newTg.Direction));
            Assert.That(tg.NonceMasking, Is.EqualTo(newTg.NonceMasking));
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
            Assert.That(tc.Key, Is.EqualTo(newTc.Key));
            Assert.That(tc.Nonce, Is.EqualTo(newTc.Nonce));
            Assert.That(tc.AD, Is.EqualTo(newTc.AD));
            Assert.That(tc.PayloadBitLength, Is.EqualTo(newTc.PayloadBitLength));
            Assert.That(tc.ADBitLength, Is.EqualTo(newTc.ADBitLength));
            Assert.That(tc.TagLength, Is.EqualTo(newTc.TagLength));
            Assert.That(tc.SecondKey, Is.EqualTo(newTc.SecondKey));
        });

        if (tc.ParentGroup.Direction == BlockCipherDirections.Encrypt)
        {
            Assert.Multiple(() =>
            {
                Assert.That(tc.Ciphertext, Is.Not.EqualTo(newTc.Ciphertext));
                Assert.That(tc.Tag, Is.Not.EqualTo(newTc.Tag));
                Assert.That(tc.Plaintext, Is.EqualTo(newTc.Plaintext));
            });
        }
        else
        {
            Assert.Multiple(() =>
            {
                Assert.That(tc.Plaintext, Is.Not.EqualTo(newTc.Plaintext));
                Assert.That(tc.Ciphertext, Is.EqualTo(newTc.Ciphertext));
                Assert.That(tc.Tag, Is.EqualTo(newTc.Tag));
            });
        }
        
        // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
        Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
        Assert.That(regex.Matches(json), Is.Empty);
    }
}
