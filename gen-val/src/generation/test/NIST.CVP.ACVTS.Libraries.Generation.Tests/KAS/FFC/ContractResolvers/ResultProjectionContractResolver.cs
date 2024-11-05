using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.FFC.ContractResolvers
{
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
        [TestCase("aft", KasMode.NoKdfNoKc, KeyAgreementMacType.None, KeyConfirmationRole.None, KeyConfirmationDirection.None)]
        [TestCase("val", KasMode.NoKdfNoKc, KeyAgreementMacType.None, KeyConfirmationRole.None, KeyConfirmationDirection.None)]
        [TestCase("aft", KasMode.KdfNoKc, KeyAgreementMacType.AesCcm, KeyConfirmationRole.None, KeyConfirmationDirection.None)]
        [TestCase("val", KasMode.KdfNoKc, KeyAgreementMacType.AesCcm, KeyConfirmationRole.None, KeyConfirmationDirection.None)]
        [TestCase("aft", KasMode.KdfNoKc, KeyAgreementMacType.HmacSha2D224, KeyConfirmationRole.None, KeyConfirmationDirection.None)]
        [TestCase("val", KasMode.KdfNoKc, KeyAgreementMacType.HmacSha2D224, KeyConfirmationRole.None, KeyConfirmationDirection.None)]
        [TestCase("aft", KasMode.KdfKc, KeyAgreementMacType.AesCcm, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase("val", KasMode.KdfKc, KeyAgreementMacType.AesCcm, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase("aft", KasMode.KdfKc, KeyAgreementMacType.HmacSha2D224, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase("val", KasMode.KdfKc, KeyAgreementMacType.HmacSha2D224, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        public void ShouldSerializeGroupProperties(string testType, KasMode kasMode, KeyAgreementMacType macType, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var tvs = TestDataMother.GetTestGroups(1, false, testType, kasMode, macType, kcRole, kcType);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

            Assert.That(newTg.Scheme, Is.Not.EqualTo(tg.Scheme), nameof(newTg.Scheme));
            Assert.That(newTg.ParmSet, Is.Not.EqualTo(tg.ParmSet), nameof(newTg.ParmSet));
            Assert.That(newTg.HashAlgName, Is.Not.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.P, Is.Not.EqualTo(tg.P), nameof(newTg.P));
            Assert.That(newTg.Q, Is.Not.EqualTo(tg.Q), nameof(newTg.Q));
            Assert.That(newTg.G, Is.Not.EqualTo(tg.G), nameof(newTg.G));

            Assert.That(json.ToLower().Contains(nameof(TestGroup.KasRole).ToLower()), Is.False, nameof(TestGroup.KasRole));
            Assert.That(json.ToLower().Contains(nameof(TestGroup.KasMode).ToLower()), Is.False, nameof(TestGroup.KasMode));
            Assert.That(json.ToLower().Contains(nameof(TestGroup.MacType).ToLower()), Is.False, nameof(TestGroup.MacType));
            Assert.That(json.ToLower().Contains(nameof(TestGroup.KcRole).ToLower()), Is.False, nameof(TestGroup.KcRole));
            Assert.That(json.ToLower().Contains(nameof(TestGroup.KcType).ToLower()), Is.False, nameof(TestGroup.KcType));

            Assert.That(newTg.KeyLen, Is.Not.EqualTo(tg.KeyLen), nameof(newTg.KeyLen));
            Assert.That(newTg.MacLen, Is.Not.EqualTo(tg.MacLen), nameof(newTg.MacLen));
            Assert.That(newTg.KdfType, Is.Not.EqualTo(tg.KdfType), nameof(newTg.KdfType));
            Assert.That(newTg.IdServerLen, Is.Not.EqualTo(tg.IdServerLen), nameof(newTg.IdServerLen));
            Assert.That(newTg.IdServer, Is.Not.EqualTo(tg.IdServer), nameof(newTg.IdServer));
            Assert.That(newTg.IdIutLen, Is.Not.EqualTo(tg.IdIutLen), nameof(newTg.IdIutLen));
            Assert.That(newTg.IdIut, Is.Not.EqualTo(tg.IdIut), nameof(newTg.IdIut));
            Assert.That(newTg.AesCcmNonceLen, Is.Not.EqualTo(tg.AesCcmNonceLen), nameof(newTg.AesCcmNonceLen));
        }

        [Test]
        [TestCase("aft", KasMode.NoKdfNoKc)]
        [TestCase("val", KasMode.NoKdfNoKc)]
        public void ShouldSerializeCasePropertiesNoKdfNoKc(string testType, KasMode kasMode)
        {
            var tvs = TestDataMother.GetTestGroups(1, false, testType, kasMode);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (testType == "val")
            {
                Assert.That(newTc.TestPassed, Is.EqualTo(tc.TestPassed), nameof(newTc.TestPassed));

                Assert.That(newTc.StaticPublicKeyIut, Is.Not.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.Not.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.Not.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.Not.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
                Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.That(regex.Matches(json).Count == 0, Is.True);

                Assert.That(newTc.StaticPublicKeyIut, Is.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            Assert.That(newTc.StaticPrivateKeyIut, Is.Not.EqualTo(tc.StaticPrivateKeyIut), nameof(newTc.StaticPrivateKeyIut));
            Assert.That(newTc.EphemeralPrivateKeyIut, Is.Not.EqualTo(tc.EphemeralPrivateKeyIut), nameof(newTc.EphemeralPrivateKeyIut));

            Assert.That(newTc.NonceAesCcm, Is.Not.EqualTo(tc.NonceAesCcm), nameof(newTc.NonceAesCcm));

            Assert.That(newTc.OiLen, Is.Not.EqualTo(tc.OiLen), nameof(newTc.OiLen));
            Assert.That(newTc.OtherInfo, Is.Not.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));
            Assert.That(newTc.Tag, Is.Not.EqualTo(tc.Tag), nameof(newTc.Tag));

            if (testType == "aft")
            {
                Assert.That(newTc.HashZ, Is.EqualTo(tc.HashZ), nameof(newTc.HashZ));
            }
            else
            {
                Assert.That(newTc.HashZ, Is.Not.EqualTo(tc.HashZ), nameof(newTc.HashZ));
            }

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);
        }
    }
}
