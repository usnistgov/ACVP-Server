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
        [TestCase(KasMode.NoKdfNoKc, "aft")]
        [TestCase(KasMode.NoKdfNoKc, "val")]
        public void ShouldSerializeGroupPropertiesNoKdfNoKc(KasMode kasMode, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(
                1,
                false,
                testType,
                kasMode,
                KeyAgreementMacType.None,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None
            );
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Scheme, Is.EqualTo(tg.Scheme), nameof(newTg.Scheme));
            Assert.That(newTg.KasRole, Is.EqualTo(tg.KasRole), nameof(newTg.KasRole));
            Assert.That(newTg.KasMode, Is.EqualTo(tg.KasMode), nameof(newTg.KasMode));
            Assert.That(newTg.ParmSet, Is.EqualTo(tg.ParmSet), nameof(newTg.ParmSet));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.P, Is.EqualTo(tg.P), nameof(newTg.P));
            Assert.That(newTg.Q, Is.EqualTo(tg.Q), nameof(newTg.Q));
            Assert.That(newTg.G, Is.EqualTo(tg.G), nameof(newTg.G));

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
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.AesCcm, "aft")]
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.HmacSha2D224, "aft")]
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.AesCcm, "val")]
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.HmacSha2D224, "val")]
        public void ShouldSerializeGroupPropertiesKdfNoKc(KasMode kasMode, KeyAgreementMacType macType, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(
                1,
                false,
                testType,
                kasMode,
                macType,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None
            );
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Scheme, Is.EqualTo(tg.Scheme), nameof(newTg.Scheme));
            Assert.That(newTg.KasRole, Is.EqualTo(tg.KasRole), nameof(newTg.KasRole));
            Assert.That(newTg.KasMode, Is.EqualTo(tg.KasMode), nameof(newTg.KasMode));
            Assert.That(newTg.ParmSet, Is.EqualTo(tg.ParmSet), nameof(newTg.ParmSet));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.P, Is.EqualTo(tg.P), nameof(newTg.P));
            Assert.That(newTg.Q, Is.EqualTo(tg.Q), nameof(newTg.Q));
            Assert.That(newTg.G, Is.EqualTo(tg.G), nameof(newTg.G));

            Assert.That(newTg.MacType, Is.EqualTo(tg.MacType), nameof(newTg.MacType));
            Assert.That(newTg.KeyLen, Is.EqualTo(tg.KeyLen), nameof(newTg.KeyLen));
            Assert.That(newTg.MacLen, Is.EqualTo(tg.MacLen), nameof(newTg.MacLen));
            Assert.That(newTg.KdfType, Is.EqualTo(tg.KdfType), nameof(newTg.KdfType));
            Assert.That(newTg.IdServerLen, Is.EqualTo(tg.IdServerLen), nameof(newTg.IdServerLen));
            Assert.That(newTg.IdServer, Is.EqualTo(tg.IdServer), nameof(newTg.IdServer));

            Assert.That(json.ToLower().Contains(nameof(TestGroup.KcRole).ToLower()), Is.False, nameof(TestGroup.KcRole));
            Assert.That(json.ToLower().Contains(nameof(TestGroup.KcType).ToLower()), Is.False, nameof(TestGroup.KcType));

            if (testType == "val")
            {
                Assert.That(newTg.IdIutLen, Is.EqualTo(tg.IdIutLen), nameof(newTg.IdIutLen));
                Assert.That(newTg.IdIut, Is.EqualTo(tg.IdIut), nameof(newTg.IdIut));
            }

            if (testType == "aft")
            {
                Assert.That(newTg.IdIutLen, Is.Not.EqualTo(tg.IdIutLen), nameof(newTg.IdIutLen));
                Assert.That(newTg.IdIut, Is.Not.EqualTo(tg.IdIut), nameof(newTg.IdIut));
            }

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.That(newTg.AesCcmNonceLen, Is.EqualTo(tg.AesCcmNonceLen), nameof(newTg.AesCcmNonceLen));
            }
            else
            {
                Assert.That(newTg.AesCcmNonceLen, Is.Not.EqualTo(tg.AesCcmNonceLen), nameof(newTg.AesCcmNonceLen));
            }

        }

        [Test]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.AesCcm, "aft")]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.HmacSha2D224, "aft")]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.AesCcm, "val")]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.HmacSha2D224, "val")]
        public void ShouldSerializeGroupPropertiesKdfKc(KasMode kasMode, KeyAgreementMacType macType, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(
                1,
                false,
                testType,
                kasMode,
                macType,
                KeyConfirmationRole.Provider,
                KeyConfirmationDirection.Unilateral
            );
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Scheme, Is.EqualTo(tg.Scheme), nameof(newTg.Scheme));
            Assert.That(newTg.KasRole, Is.EqualTo(tg.KasRole), nameof(newTg.KasRole));
            Assert.That(newTg.KasMode, Is.EqualTo(tg.KasMode), nameof(newTg.KasMode));
            Assert.That(newTg.ParmSet, Is.EqualTo(tg.ParmSet), nameof(newTg.ParmSet));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.P, Is.EqualTo(tg.P), nameof(newTg.P));
            Assert.That(newTg.Q, Is.EqualTo(tg.Q), nameof(newTg.Q));
            Assert.That(newTg.G, Is.EqualTo(tg.G), nameof(newTg.G));

            Assert.That(newTg.MacType, Is.EqualTo(tg.MacType), nameof(newTg.MacType));
            Assert.That(newTg.KeyLen, Is.EqualTo(tg.KeyLen), nameof(newTg.KeyLen));
            Assert.That(newTg.MacLen, Is.EqualTo(tg.MacLen), nameof(newTg.MacLen));
            Assert.That(newTg.KdfType, Is.EqualTo(tg.KdfType), nameof(newTg.KdfType));
            Assert.That(newTg.IdServerLen, Is.EqualTo(tg.IdServerLen), nameof(newTg.IdServerLen));
            Assert.That(newTg.IdServer, Is.EqualTo(tg.IdServer), nameof(newTg.IdServer));
            Assert.That(newTg.KcRole, Is.EqualTo(tg.KcRole), nameof(newTg.KcRole));
            Assert.That(newTg.KcType, Is.EqualTo(tg.KcType), nameof(newTg.KcType));

            if (testType == "val")
            {
                Assert.That(newTg.IdIutLen, Is.EqualTo(tg.IdIutLen), nameof(newTg.IdIutLen));
                Assert.That(newTg.IdIut, Is.EqualTo(tg.IdIut), nameof(newTg.IdIut));
            }

            if (testType == "aft")
            {
                Assert.That(newTg.IdIutLen, Is.Not.EqualTo(tg.IdIutLen), nameof(newTg.IdIutLen));
                Assert.That(newTg.IdIut, Is.Not.EqualTo(tg.IdIut), nameof(newTg.IdIut));
            }

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.That(newTg.AesCcmNonceLen, Is.EqualTo(tg.AesCcmNonceLen), nameof(newTg.AesCcmNonceLen));
            }
            else
            {
                Assert.That(newTg.AesCcmNonceLen, Is.Not.EqualTo(tg.AesCcmNonceLen), nameof(newTg.AesCcmNonceLen));
            }

        }

        [Test]
        [TestCase(KasMode.NoKdfNoKc, "aft")]
        [TestCase(KasMode.NoKdfNoKc, "val")]
        public void ShouldSerializeCasePropertiesNoKdfNoKc(KasMode kasMode, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(
                1,
                false,
                testType,
                kasMode,
                KeyAgreementMacType.None,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None
            );
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.StaticPublicKeyServer, Is.EqualTo(tc.StaticPublicKeyServer), nameof(newTc.StaticPublicKeyServer));
            Assert.That(newTc.EphemeralPublicKeyServer, Is.EqualTo(tc.EphemeralPublicKeyServer), nameof(newTc.EphemeralPublicKeyServer));
            Assert.That(newTc.DkmNonceServer, Is.EqualTo(tc.DkmNonceServer), nameof(newTc.DkmNonceServer));
            Assert.That(newTc.EphemeralNonceServer, Is.EqualTo(tc.EphemeralNonceServer), nameof(newTc.EphemeralNonceServer));
            Assert.That(newTc.NonceNoKc, Is.EqualTo(tc.NonceNoKc), nameof(newTc.NonceNoKc));

            Assert.That(newTc.NonceAesCcm, Is.Not.EqualTo(tc.NonceAesCcm), nameof(newTc.NonceAesCcm));

            if (testType == "val")
            {
                Assert.That(newTc.StaticPrivateKeyIut, Is.EqualTo(tc.StaticPrivateKeyIut), nameof(newTc.StaticPrivateKeyIut));
                Assert.That(newTc.StaticPublicKeyIut, Is.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPrivateKeyIut, Is.EqualTo(tc.EphemeralPrivateKeyIut), nameof(newTc.EphemeralPrivateKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                Assert.That(newTc.StaticPrivateKeyIut, Is.Not.EqualTo(tc.StaticPrivateKeyIut), nameof(newTc.StaticPrivateKeyIut));
                Assert.That(newTc.StaticPublicKeyIut, Is.Not.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPrivateKeyIut, Is.Not.EqualTo(tc.EphemeralPrivateKeyIut), nameof(newTc.EphemeralPrivateKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.Not.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.Not.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.Not.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            Assert.That(newTc.OiLen, Is.Not.EqualTo(tc.OiLen), nameof(newTc.OiLen));
            Assert.That(newTc.OtherInfo, Is.Not.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));
            Assert.That(newTc.Tag, Is.Not.EqualTo(tc.Tag), nameof(newTc.Tag));

            if (testType == "val")
            {
                Assert.That(newTc.HashZ, Is.EqualTo(tc.HashZ), nameof(newTc.HashZ));
            }
            else
            {
                Assert.That(newTc.HashZ, Is.Not.EqualTo(tc.HashZ), nameof(newTc.HashZ));
            }


            Assert.That(newTc.Z, Is.Not.EqualTo(tc.Z), nameof(newTc.Z));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regexTestPassed.Matches(json).Count == 0, Is.True);
        }

        [Test]
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.AesCcm, "aft")]
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.AesCcm, "val")]
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.HmacSha2D384, "aft")]
        [TestCase(KasMode.KdfNoKc, KeyAgreementMacType.HmacSha2D384, "val")]
        public void ShouldSerializeCasePropertiesKdfNoKc(KasMode kasMode, KeyAgreementMacType macType, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(
                1,
                false,
                testType,
                kasMode,
                macType,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None
            );
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.StaticPublicKeyServer, Is.EqualTo(tc.StaticPublicKeyServer), nameof(newTc.StaticPublicKeyServer));
            Assert.That(newTc.EphemeralPublicKeyServer, Is.EqualTo(tc.EphemeralPublicKeyServer), nameof(newTc.EphemeralPublicKeyServer));
            Assert.That(newTc.DkmNonceServer, Is.EqualTo(tc.DkmNonceServer), nameof(newTc.DkmNonceServer));
            Assert.That(newTc.EphemeralNonceServer, Is.EqualTo(tc.EphemeralNonceServer), nameof(newTc.EphemeralNonceServer));
            Assert.That(newTc.NonceNoKc, Is.EqualTo(tc.NonceNoKc), nameof(newTc.NonceNoKc));

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.That(newTc.NonceAesCcm, Is.EqualTo(tc.NonceAesCcm), nameof(newTc.NonceAesCcm));
            }
            else
            {
                Assert.That(newTc.NonceAesCcm, Is.Not.EqualTo(tc.NonceAesCcm), nameof(newTc.NonceAesCcm));
            }


            if (testType == "val")
            {
                Assert.That(newTc.StaticPrivateKeyIut, Is.EqualTo(tc.StaticPrivateKeyIut), nameof(newTc.StaticPrivateKeyIut));
                Assert.That(newTc.StaticPublicKeyIut, Is.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPrivateKeyIut, Is.EqualTo(tc.EphemeralPrivateKeyIut), nameof(newTc.EphemeralPrivateKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                Assert.That(newTc.StaticPrivateKeyIut, Is.Not.EqualTo(tc.StaticPrivateKeyIut), nameof(newTc.StaticPrivateKeyIut));
                Assert.That(newTc.StaticPublicKeyIut, Is.Not.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPrivateKeyIut, Is.Not.EqualTo(tc.EphemeralPrivateKeyIut), nameof(newTc.EphemeralPrivateKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.Not.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.Not.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.Not.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "val")
            {
                Assert.That(newTc.OiLen, Is.EqualTo(tc.OiLen), nameof(newTc.OiLen));
                Assert.That(newTc.OtherInfo, Is.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));
                Assert.That(newTc.Tag, Is.EqualTo(tc.Tag), nameof(newTc.Tag));
            }

            if (testType == "aft")
            {
                Assert.That(newTc.OiLen, Is.Not.EqualTo(tc.OiLen), nameof(newTc.OiLen));
                Assert.That(newTc.OtherInfo, Is.Not.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));
                Assert.That(newTc.Tag, Is.Not.EqualTo(tc.Tag), nameof(newTc.Tag));
            }

            Assert.That(newTc.Z, Is.Not.EqualTo(tc.Z), nameof(newTc.Z));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regexTestPassed.Matches(json).Count == 0, Is.True);
        }

        [Test]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.AesCcm, "aft")]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.AesCcm, "val")]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.HmacSha2D384, "aft")]
        [TestCase(KasMode.KdfKc, KeyAgreementMacType.HmacSha2D384, "val")]
        public void ShouldSerializeCasePropertiesKdfKc(KasMode kasMode, KeyAgreementMacType macType, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(
                1,
                false,
                testType,
                kasMode,
                macType,
                KeyConfirmationRole.Provider,
                KeyConfirmationDirection.Bilateral
            );
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.StaticPublicKeyServer, Is.EqualTo(tc.StaticPublicKeyServer), nameof(newTc.StaticPublicKeyServer));
            Assert.That(newTc.EphemeralPublicKeyServer, Is.EqualTo(tc.EphemeralPublicKeyServer), nameof(newTc.EphemeralPublicKeyServer));
            Assert.That(newTc.DkmNonceServer, Is.EqualTo(tc.DkmNonceServer), nameof(newTc.DkmNonceServer));
            Assert.That(newTc.EphemeralNonceServer, Is.EqualTo(tc.EphemeralNonceServer), nameof(newTc.EphemeralNonceServer));
            Assert.That(newTc.NonceNoKc, Is.EqualTo(tc.NonceNoKc), nameof(newTc.NonceNoKc));

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.That(newTc.NonceAesCcm, Is.EqualTo(tc.NonceAesCcm), nameof(newTc.NonceAesCcm));
            }
            else
            {
                Assert.That(newTc.NonceAesCcm, Is.Not.EqualTo(tc.NonceAesCcm), nameof(newTc.NonceAesCcm));
            }


            if (testType == "val")
            {
                Assert.That(newTc.StaticPrivateKeyIut, Is.EqualTo(tc.StaticPrivateKeyIut), nameof(newTc.StaticPrivateKeyIut));
                Assert.That(newTc.StaticPublicKeyIut, Is.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPrivateKeyIut, Is.EqualTo(tc.EphemeralPrivateKeyIut), nameof(newTc.EphemeralPrivateKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                Assert.That(newTc.StaticPrivateKeyIut, Is.Not.EqualTo(tc.StaticPrivateKeyIut), nameof(newTc.StaticPrivateKeyIut));
                Assert.That(newTc.StaticPublicKeyIut, Is.Not.EqualTo(tc.StaticPublicKeyIut), nameof(newTc.StaticPublicKeyIut));
                Assert.That(newTc.EphemeralPrivateKeyIut, Is.Not.EqualTo(tc.EphemeralPrivateKeyIut), nameof(newTc.EphemeralPrivateKeyIut));
                Assert.That(newTc.EphemeralPublicKeyIut, Is.Not.EqualTo(tc.EphemeralPublicKeyIut), nameof(newTc.EphemeralPublicKeyIut));
                Assert.That(newTc.DkmNonceIut, Is.Not.EqualTo(tc.DkmNonceIut), nameof(newTc.DkmNonceIut));
                Assert.That(newTc.EphemeralNonceIut, Is.Not.EqualTo(tc.EphemeralNonceIut), nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "val")
            {
                Assert.That(newTc.OiLen, Is.EqualTo(tc.OiLen), nameof(newTc.OiLen));
                Assert.That(newTc.OtherInfo, Is.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));
                Assert.That(newTc.Tag, Is.EqualTo(tc.Tag), nameof(newTc.Tag));
            }

            if (testType == "aft")
            {
                Assert.That(newTc.OiLen, Is.Not.EqualTo(tc.OiLen), nameof(newTc.OiLen));
                Assert.That(newTc.OtherInfo, Is.Not.EqualTo(tc.OtherInfo), nameof(newTc.OtherInfo));
                Assert.That(newTc.Tag, Is.Not.EqualTo(tc.Tag), nameof(newTc.Tag));
            }

            Assert.That(newTc.Z, Is.Not.EqualTo(tc.Z), nameof(newTc.Z));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regexTestPassed.Matches(json).Count == 0, Is.True);
        }
    }
}
