using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KMAC.ContractResolvers
{
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
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.XOF, Is.EqualTo(tg.XOF), nameof(newTg.XOF));
            Assert.That(newTg.HexCustomization, Is.EqualTo(tg.HexCustomization), nameof(newTg.HexCustomization));
        }

        [Test]
        [TestCase("aft", true)]
        [TestCase("aft", false)]
        [TestCase("mvt", true)]
        [TestCase("mvt", false)]
        public void ShouldSerializeCaseProperties(string testType, bool hexCustomization)
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];
            tg.TestType = testType;
            tg.HexCustomization = hexCustomization;
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.KeyLength, Is.EqualTo(tc.KeyLength), nameof(newTc.KeyLength));
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
            Assert.That(newTc.MessageLength, Is.EqualTo(tc.MessageLength), nameof(newTc.MessageLength));
            Assert.That(newTc.MacLength, Is.EqualTo(tc.MacLength), nameof(newTc.MacLength));

            if (testType == "mvt")
            {
                Assert.That(newTc.Mac, Is.EqualTo(tc.Mac), nameof(newTc.Mac));
            }
            else
            {
                Assert.That(newTc.Mac, Is.Not.EqualTo(tc.Mac), nameof(newTc.Mac));
            }

            if (hexCustomization)
            {
                Assert.That(newTc.Customization, Is.Not.EqualTo(tc.Customization), nameof(newTc.Customization));
                Assert.That(newTc.CustomizationHex, Is.EqualTo(tc.CustomizationHex), nameof(newTc.CustomizationHex));
            }
            else
            {
                Assert.That(newTc.Customization, Is.EqualTo(tc.Customization), nameof(newTc.Customization));
                Assert.That(newTc.CustomizationHex, Is.Not.EqualTo(tc.CustomizationHex), nameof(newTc.CustomizationHex));
            }

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regexTestPassed.Matches(json).Count == 0, Is.True);
        }
    }
}
