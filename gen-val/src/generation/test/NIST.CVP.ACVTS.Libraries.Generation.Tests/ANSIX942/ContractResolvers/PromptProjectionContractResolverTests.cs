using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX942.ContractResolvers
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
        [TestCase(AnsiX942Types.Concat)]
        [TestCase(AnsiX942Types.Der)]
        public void ShouldSerializeGroupProperties(AnsiX942Types kdfType)
        {
            var tvs = TestDataMother.GetTestGroups(kdfType);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.KdfType, Is.EqualTo(tg.KdfType), nameof(newTg.KdfType));

            if (kdfType == AnsiX942Types.Der)
            {
                Assert.That(newTg.Oid, Is.EqualTo(tg.Oid), nameof(newTg.Oid));
            }
        }

        [Test]
        [TestCase(AnsiX942Types.Concat)]
        [TestCase(AnsiX942Types.Der)]
        public void ShouldSerializeCaseProperties(AnsiX942Types kdfType)
        {
            var tvs = TestDataMother.GetTestGroups(kdfType);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Zz, Is.EqualTo(tc.Zz), nameof(newTc.Zz));

            if (kdfType == AnsiX942Types.Concat)
            {
                Assert.That(newTc.OtherInfo, Is.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));

                Assert.That(newTc.PartyUInfo, Is.Not.EqualTo(tc.PartyUInfo), nameof(newTc.PartyUInfo));
                Assert.That(newTc.PartyVInfo, Is.Not.EqualTo(tc.PartyVInfo), nameof(newTc.PartyVInfo));
                Assert.That(newTc.SuppPubInfo, Is.Not.EqualTo(tc.SuppPubInfo), nameof(newTc.SuppPubInfo));
                Assert.That(newTc.SuppPrivInfo, Is.Not.EqualTo(tc.SuppPrivInfo), nameof(newTc.SuppPrivInfo));
            }
            else
            {
                Assert.That(newTc.PartyUInfo, Is.EqualTo(tc.PartyUInfo), nameof(newTc.PartyUInfo));
                Assert.That(newTc.PartyVInfo, Is.EqualTo(tc.PartyVInfo), nameof(newTc.PartyVInfo));
                Assert.That(newTc.SuppPubInfo, Is.EqualTo(tc.SuppPubInfo), nameof(newTc.SuppPubInfo));
                Assert.That(newTc.SuppPrivInfo, Is.EqualTo(tc.SuppPrivInfo), nameof(newTc.SuppPrivInfo));

                Assert.That(newTc.OtherInfo, Is.Not.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));
            }

            Assert.That(newTc.DerivedKey, Is.Not.EqualTo(tc.DerivedKey), nameof(newTc.DerivedKey));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
