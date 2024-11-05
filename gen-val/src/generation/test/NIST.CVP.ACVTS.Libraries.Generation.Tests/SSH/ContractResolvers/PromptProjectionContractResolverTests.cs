using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SSH.ContractResolvers
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
            Assert.That(newTg.Cipher, Is.EqualTo(tg.Cipher), nameof(newTg.Cipher));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
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
            Assert.That(newTc.K, Is.EqualTo(tc.K), nameof(newTc.K));
            Assert.That(newTc.H, Is.EqualTo(tc.H), nameof(newTc.H));
            Assert.That(newTc.SessionId, Is.EqualTo(tc.SessionId), nameof(newTc.SessionId));

            Assert.That(newTc.InitialIvClient, Is.Not.EqualTo(tc.InitialIvClient), nameof(newTc.InitialIvClient));
            Assert.That(newTc.EncryptionKeyClient, Is.Not.EqualTo(tc.EncryptionKeyClient), nameof(newTc.EncryptionKeyClient));
            Assert.That(newTc.IntegrityKeyClient, Is.Not.EqualTo(tc.IntegrityKeyClient), nameof(newTc.IntegrityKeyClient));
            Assert.That(newTc.InitialIvServer, Is.Not.EqualTo(tc.InitialIvServer), nameof(newTc.InitialIvServer));
            Assert.That(newTc.EncryptionKeyServer, Is.Not.EqualTo(tc.EncryptionKeyServer), nameof(newTc.EncryptionKeyServer));
            Assert.That(newTc.IntegrityKeyServer, Is.Not.EqualTo(tc.IntegrityKeyServer), nameof(newTc.IntegrityKeyServer));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
