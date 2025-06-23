using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v2_0;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v2_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.HMAC.v2_0.ContractResolvers
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class PromptProjectionContractResolverTests
    {
        private readonly JsonConverterProvider _jsonConverterProvider = new ();
        private readonly ContractResolverFactory _contractResolverFactory = new ();
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
        }

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

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.KeyLen, Is.EqualTo(tc.KeyLen), nameof(newTc.KeyLen));
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
            Assert.That(newTc.MessageLen, Is.EqualTo(tc.MessageLen), nameof(newTc.MessageLen));
            Assert.That(newTc.MacLen, Is.EqualTo(tc.MacLen), nameof(newTc.MacLen));

            Assert.That(newTc.Mac, Is.Not.EqualTo(tc.Mac), nameof(newTc.Mac));

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regexTestPassed.Matches(json).Count == 0, Is.True);
        }
    }
}
