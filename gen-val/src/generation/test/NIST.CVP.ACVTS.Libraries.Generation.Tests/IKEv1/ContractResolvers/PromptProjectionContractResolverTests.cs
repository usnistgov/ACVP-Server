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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
            Assert.AreEqual(tg.AuthenticationMethod, newTg.AuthenticationMethod, nameof(newTg.AuthenticationMethod));
            Assert.AreEqual(tg.NInitLength, newTg.NInitLength, nameof(newTg.NInitLength));
            Assert.AreEqual(tg.NRespLength, newTg.NRespLength, nameof(newTg.NRespLength));
            Assert.AreEqual(tg.GxyLength, newTg.GxyLength, nameof(newTg.GxyLength));

            if (authMethod == AuthenticationMethods.Psk)
            {
                Assert.AreEqual(tg.PreSharedKeyLength, newTg.PreSharedKeyLength, nameof(newTg.PreSharedKeyLength));
            }
            else
            {
                Assert.AreNotEqual(tg.PreSharedKeyLength, newTg.PreSharedKeyLength, nameof(newTg.PreSharedKeyLength));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.NInit, newTc.NInit, nameof(newTc.NInit));
            Assert.AreEqual(tc.NResp, newTc.NResp, nameof(newTc.NResp));
            Assert.AreEqual(tc.CkyInit, newTc.CkyInit, nameof(newTc.CkyInit));
            Assert.AreEqual(tc.CkyResp, newTc.CkyResp, nameof(newTc.CkyResp));
            Assert.AreEqual(tc.Gxy, newTc.Gxy, nameof(newTc.Gxy));

            if (authMethod == AuthenticationMethods.Psk)
            {
                Assert.AreEqual(tc.PreSharedKey, newTc.PreSharedKey, nameof(newTc.PreSharedKey));
            }
            else
            {
                Assert.AreNotEqual(tc.PreSharedKey, newTc.PreSharedKey, nameof(newTc.PreSharedKey));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
