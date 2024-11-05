using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.IKEv1.ContractResolvers
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
        [TestCase(AuthenticationMethods.Dsa)]
        [TestCase(AuthenticationMethods.Psk)]
        public void ShouldSerializeGroupProperties(AuthenticationMethods authMethod)
        {
            var tvs = TestDataMother.GetTestGroups(1, authMethod);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.AuthenticationMethod, Is.EqualTo(tg.AuthenticationMethod), nameof(newTg.AuthenticationMethod));
            Assert.That(newTg.NInitLength, Is.EqualTo(tg.NInitLength), nameof(newTg.NInitLength));
            Assert.That(newTg.NRespLength, Is.EqualTo(tg.NRespLength), nameof(newTg.NRespLength));
            Assert.That(newTg.GxyLength, Is.EqualTo(tg.GxyLength), nameof(newTg.GxyLength));

            if (authMethod == AuthenticationMethods.Psk)
            {
                Assert.That(newTg.PreSharedKeyLength, Is.EqualTo(tg.PreSharedKeyLength), nameof(newTg.PreSharedKeyLength));
            }
            else
            {
                Assert.That(newTg.PreSharedKeyLength, Is.Not.EqualTo(tg.PreSharedKeyLength), nameof(newTg.PreSharedKeyLength));
            }
        }

        [Test]
        [TestCase(AuthenticationMethods.Dsa)]
        [TestCase(AuthenticationMethods.Psk)]
        public void ShouldSerializeCaseProperties(AuthenticationMethods authMethod)
        {
            var tvs = TestDataMother.GetTestGroups(1, authMethod);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.NInit, Is.EqualTo(tc.NInit), nameof(newTc.NInit));
            Assert.That(newTc.NResp, Is.EqualTo(tc.NResp), nameof(newTc.NResp));
            Assert.That(newTc.CkyInit, Is.EqualTo(tc.CkyInit), nameof(newTc.CkyInit));
            Assert.That(newTc.CkyResp, Is.EqualTo(tc.CkyResp), nameof(newTc.CkyResp));
            Assert.That(newTc.Gxy, Is.EqualTo(tc.Gxy), nameof(newTc.Gxy));

            if (authMethod == AuthenticationMethods.Psk)
            {
                Assert.That(newTc.PreSharedKey, Is.EqualTo(tc.PreSharedKey), nameof(newTc.PreSharedKey));
            }
            else
            {
                Assert.That(newTc.PreSharedKey, Is.Not.EqualTo(tc.PreSharedKey), nameof(newTc.PreSharedKey));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
