using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups(1);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.TlsMode, Is.EqualTo(tg.TlsMode), nameof(newTg.TlsMode));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.PreMasterSecretLength, Is.EqualTo(tg.PreMasterSecretLength), nameof(newTg.PreMasterSecretLength));
            Assert.That(newTg.KeyBlockLength, Is.EqualTo(tg.KeyBlockLength), nameof(newTg.KeyBlockLength));
        }

        [Test]
        public void ShouldSerializeCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.ClientHelloRandom, Is.EqualTo(tc.ClientHelloRandom), nameof(newTc.ClientHelloRandom));
            Assert.That(newTc.ServerHelloRandom, Is.EqualTo(tc.ServerHelloRandom), nameof(newTc.ServerHelloRandom));
            Assert.That(newTc.ClientRandom, Is.EqualTo(tc.ClientRandom), nameof(newTc.ClientRandom));
            Assert.That(newTc.ServerRandom, Is.EqualTo(tc.ServerRandom), nameof(newTc.ServerRandom));
            Assert.That(newTc.PreMasterSecret, Is.EqualTo(tc.PreMasterSecret), nameof(newTc.PreMasterSecret));

            Assert.That(newTc.MasterSecret, Is.Not.EqualTo(tc.MasterSecret), nameof(newTc.MasterSecret));
            Assert.That(newTc.KeyBlock, Is.Not.EqualTo(tc.KeyBlock), nameof(newTc.KeyBlock));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
