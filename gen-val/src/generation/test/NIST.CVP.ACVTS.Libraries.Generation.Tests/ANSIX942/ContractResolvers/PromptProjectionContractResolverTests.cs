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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
            Assert.AreEqual(tg.KdfType, newTg.KdfType, nameof(newTg.KdfType));

            if (kdfType == AnsiX942Types.Der)
            {
                Assert.AreEqual(tg.Oid, newTg.Oid, nameof(newTg.Oid));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Zz, newTc.Zz, nameof(newTc.Zz));

            if (kdfType == AnsiX942Types.Concat)
            {
                Assert.AreEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));

                Assert.AreNotEqual(tc.PartyUInfo, newTc.PartyUInfo, nameof(newTc.PartyUInfo));
                Assert.AreNotEqual(tc.PartyVInfo, newTc.PartyVInfo, nameof(newTc.PartyVInfo));
                Assert.AreNotEqual(tc.SuppPubInfo, newTc.SuppPubInfo, nameof(newTc.SuppPubInfo));
                Assert.AreNotEqual(tc.SuppPrivInfo, newTc.SuppPrivInfo, nameof(newTc.SuppPrivInfo));
            }
            else
            {
                Assert.AreEqual(tc.PartyUInfo, newTc.PartyUInfo, nameof(newTc.PartyUInfo));
                Assert.AreEqual(tc.PartyVInfo, newTc.PartyVInfo, nameof(newTc.PartyVInfo));
                Assert.AreEqual(tc.SuppPubInfo, newTc.SuppPubInfo, nameof(newTc.SuppPubInfo));
                Assert.AreEqual(tc.SuppPrivInfo, newTc.SuppPrivInfo, nameof(newTc.SuppPrivInfo));

                Assert.AreNotEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));
            }

            Assert.AreNotEqual(tc.DerivedKey, newTc.DerivedKey, nameof(newTc.DerivedKey));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
