using System;
using System.Text.RegularExpressions;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.KAS.ECC.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Scheme, newTg.Scheme, nameof(newTg.Scheme));
            Assert.AreEqual(tg.KasRole, newTg.KasRole, nameof(newTg.KasRole));
            Assert.AreEqual(tg.KasMode, newTg.KasMode, nameof(newTg.KasMode));
            Assert.AreEqual(tg.ParmSet, newTg.ParmSet, nameof(newTg.ParmSet));
            Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
            Assert.AreEqual(tg.CurveName, newTg.CurveName, nameof(newTg.CurveName));

            Assert.IsFalse(json.ToLower().Contains(nameof(TestGroup.MacType).ToLower()), nameof(TestGroup.MacType));
            Assert.IsFalse(json.ToLower().Contains(nameof(TestGroup.KcRole).ToLower()), nameof(TestGroup.KcRole));
            Assert.IsFalse(json.ToLower().Contains(nameof(TestGroup.KcType).ToLower()), nameof(TestGroup.KcType));

            Assert.AreNotEqual(tg.KeyLen, newTg.KeyLen, nameof(newTg.KeyLen));
            Assert.AreNotEqual(tg.MacLen, newTg.MacLen, nameof(newTg.MacLen));
            Assert.AreNotEqual(tg.KdfType, newTg.KdfType, nameof(newTg.KdfType));
            Assert.AreNotEqual(tg.IdServerLen, newTg.IdServerLen, nameof(newTg.IdServerLen));
            Assert.AreNotEqual(tg.IdServer, newTg.IdServer, nameof(newTg.IdServer));
            Assert.AreNotEqual(tg.IdIutLen, newTg.IdIutLen, nameof(newTg.IdIutLen));
            Assert.AreNotEqual(tg.IdIut, newTg.IdIut, nameof(newTg.IdIut));
            Assert.AreNotEqual(tg.AesCcmNonceLen, newTg.AesCcmNonceLen, nameof(newTg.AesCcmNonceLen));
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Scheme, newTg.Scheme, nameof(newTg.Scheme));
            Assert.AreEqual(tg.KasRole, newTg.KasRole, nameof(newTg.KasRole));
            Assert.AreEqual(tg.KasMode, newTg.KasMode, nameof(newTg.KasMode));
            Assert.AreEqual(tg.ParmSet, newTg.ParmSet, nameof(newTg.ParmSet));
            Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
            Assert.AreEqual(tg.CurveName, newTg.CurveName, nameof(newTg.CurveName));

            Assert.AreEqual(tg.MacType, newTg.MacType, nameof(newTg.MacType));
            Assert.AreEqual(tg.KeyLen, newTg.KeyLen, nameof(newTg.KeyLen));
            Assert.AreEqual(tg.MacLen, newTg.MacLen, nameof(newTg.MacLen));
            Assert.AreEqual(tg.KdfType, newTg.KdfType, nameof(newTg.KdfType));
            Assert.AreEqual(tg.IdServerLen, newTg.IdServerLen, nameof(newTg.IdServerLen));
            Assert.AreEqual(tg.IdServer, newTg.IdServer, nameof(newTg.IdServer));

            Assert.IsFalse(json.ToLower().Contains(nameof(TestGroup.KcRole).ToLower()), nameof(TestGroup.KcRole));
            Assert.IsFalse(json.ToLower().Contains(nameof(TestGroup.KcType).ToLower()), nameof(TestGroup.KcType));

            if (testType == "val")
            {
                Assert.AreEqual(tg.IdIutLen, newTg.IdIutLen, nameof(newTg.IdIutLen));
                Assert.AreEqual(tg.IdIut, newTg.IdIut, nameof(newTg.IdIut));
            }

            if (testType == "aft")
            {
                Assert.AreNotEqual(tg.IdIutLen, newTg.IdIutLen, nameof(newTg.IdIutLen));
                Assert.AreNotEqual(tg.IdIut, newTg.IdIut, nameof(newTg.IdIut));
            }

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.AreEqual(tg.AesCcmNonceLen, newTg.AesCcmNonceLen, nameof(newTg.AesCcmNonceLen));
            }
            else
            {
                Assert.AreNotEqual(tg.AesCcmNonceLen, newTg.AesCcmNonceLen, nameof(newTg.AesCcmNonceLen));
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Scheme, newTg.Scheme, nameof(newTg.Scheme));
            Assert.AreEqual(tg.KasRole, newTg.KasRole, nameof(newTg.KasRole));
            Assert.AreEqual(tg.KasMode, newTg.KasMode, nameof(newTg.KasMode));
            Assert.AreEqual(tg.ParmSet, newTg.ParmSet, nameof(newTg.ParmSet));
            Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
            Assert.AreEqual(tg.CurveName, newTg.CurveName, nameof(newTg.CurveName));

            Assert.AreEqual(tg.MacType, newTg.MacType, nameof(newTg.MacType));
            Assert.AreEqual(tg.KeyLen, newTg.KeyLen, nameof(newTg.KeyLen));
            Assert.AreEqual(tg.MacLen, newTg.MacLen, nameof(newTg.MacLen));
            Assert.AreEqual(tg.KdfType, newTg.KdfType, nameof(newTg.KdfType));
            Assert.AreEqual(tg.IdServerLen, newTg.IdServerLen, nameof(newTg.IdServerLen));
            Assert.AreEqual(tg.IdServer, newTg.IdServer, nameof(newTg.IdServer));
            Assert.AreEqual(tg.KcRole, newTg.KcRole, nameof(newTg.KcRole));
            Assert.AreEqual(tg.KcType, newTg.KcType, nameof(newTg.KcType));

            if (testType == "val")
            {
                Assert.AreEqual(tg.IdIutLen, newTg.IdIutLen, nameof(newTg.IdIutLen));
                Assert.AreEqual(tg.IdIut, newTg.IdIut, nameof(newTg.IdIut));
            }

            if (testType == "aft")
            {
                Assert.AreNotEqual(tg.IdIutLen, newTg.IdIutLen, nameof(newTg.IdIutLen));
                Assert.AreNotEqual(tg.IdIut, newTg.IdIut, nameof(newTg.IdIut));
            }

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.AreEqual(tg.AesCcmNonceLen, newTg.AesCcmNonceLen, nameof(newTg.AesCcmNonceLen));
            }
            else
            {
                Assert.AreNotEqual(tg.AesCcmNonceLen, newTg.AesCcmNonceLen, nameof(newTg.AesCcmNonceLen));
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

            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.StaticPublicKeyServerX, newTc.StaticPublicKeyServerX, nameof(newTc.StaticPublicKeyServerX));
            Assert.AreEqual(tc.StaticPublicKeyServerY, newTc.StaticPublicKeyServerY, nameof(newTc.StaticPublicKeyServerY));
            Assert.AreEqual(tc.EphemeralPublicKeyServerX, newTc.EphemeralPublicKeyServerX, nameof(newTc.EphemeralPublicKeyServerX));
            Assert.AreEqual(tc.EphemeralPublicKeyServerY, newTc.EphemeralPublicKeyServerY, nameof(newTc.EphemeralPublicKeyServerY));
            Assert.AreEqual(tc.DkmNonceServer, newTc.DkmNonceServer, nameof(newTc.DkmNonceServer));
            Assert.AreEqual(tc.EphemeralNonceServer, newTc.EphemeralNonceServer, nameof(newTc.EphemeralNonceServer));
            Assert.AreEqual(tc.NonceNoKc, newTc.NonceNoKc, nameof(newTc.NonceNoKc));

            Assert.AreNotEqual(tc.NonceAesCcm, newTc.NonceAesCcm, nameof(newTc.NonceAesCcm));

            if (testType == "val")
            {
                Assert.AreEqual(tc.StaticPrivateKeyIut, newTc.StaticPrivateKeyIut, nameof(newTc.StaticPrivateKeyIut));
                Assert.AreEqual(tc.StaticPublicKeyIutX, newTc.StaticPublicKeyIutX, nameof(newTc.StaticPublicKeyIutX));
                Assert.AreEqual(tc.StaticPublicKeyIutY, newTc.StaticPublicKeyIutY, nameof(newTc.StaticPublicKeyIutY));
                Assert.AreEqual(tc.EphemeralPrivateKeyIut, newTc.EphemeralPrivateKeyIut, nameof(newTc.EphemeralPrivateKeyIut));
                Assert.AreEqual(tc.EphemeralPublicKeyIutX, newTc.EphemeralPublicKeyIutX, nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreEqual(tc.EphemeralPublicKeyIutY, newTc.EphemeralPublicKeyIutY, nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                Assert.AreNotEqual(tc.StaticPrivateKeyIut, newTc.StaticPrivateKeyIut, nameof(newTc.StaticPrivateKeyIut));
                Assert.AreNotEqual(tc.StaticPublicKeyIutX, newTc.StaticPublicKeyIutX, nameof(newTc.StaticPublicKeyIutX));
                Assert.AreNotEqual(tc.StaticPublicKeyIutY, newTc.StaticPublicKeyIutY, nameof(newTc.StaticPublicKeyIutY));
                Assert.AreNotEqual(tc.EphemeralPrivateKeyIut, newTc.EphemeralPrivateKeyIut, nameof(newTc.EphemeralPrivateKeyIut));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutX, newTc.EphemeralPublicKeyIutX, nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutY, newTc.EphemeralPublicKeyIutY, nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreNotEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreNotEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            Assert.AreNotEqual(tc.OiLen, newTc.OiLen, nameof(newTc.OiLen));
            Assert.AreNotEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));
            Assert.AreNotEqual(tc.Tag, newTc.Tag, nameof(newTc.Tag));

            if (testType == "val")
            {
                Assert.AreEqual(tc.HashZ, newTc.HashZ, nameof(newTc.HashZ));
            }
            else
            {
                Assert.AreNotEqual(tc.HashZ, newTc.HashZ, nameof(newTc.HashZ));
            }
            

            Assert.AreNotEqual(tc.Z, newTc.Z, nameof(newTc.Z));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
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

            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.StaticPublicKeyServerX, newTc.StaticPublicKeyServerX, nameof(newTc.StaticPublicKeyServerX));
            Assert.AreEqual(tc.StaticPublicKeyServerY, newTc.StaticPublicKeyServerY, nameof(newTc.StaticPublicKeyServerY));
            Assert.AreEqual(tc.EphemeralPublicKeyServerX, newTc.EphemeralPublicKeyServerX, nameof(newTc.EphemeralPublicKeyServerX));
            Assert.AreEqual(tc.EphemeralPublicKeyServerY, newTc.EphemeralPublicKeyServerY, nameof(newTc.EphemeralPublicKeyServerY));
            Assert.AreEqual(tc.DkmNonceServer, newTc.DkmNonceServer, nameof(newTc.DkmNonceServer));
            Assert.AreEqual(tc.EphemeralNonceServer, newTc.EphemeralNonceServer, nameof(newTc.EphemeralNonceServer));
            Assert.AreEqual(tc.NonceNoKc, newTc.NonceNoKc, nameof(newTc.NonceNoKc));

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.AreEqual(tc.NonceAesCcm, newTc.NonceAesCcm, nameof(newTc.NonceAesCcm));
            }
            else
            {
                Assert.AreNotEqual(tc.NonceAesCcm, newTc.NonceAesCcm, nameof(newTc.NonceAesCcm));
            }
            

            if (testType == "val")
            {
                Assert.AreEqual(tc.StaticPrivateKeyIut, newTc.StaticPrivateKeyIut, nameof(newTc.StaticPrivateKeyIut));
                Assert.AreEqual(tc.StaticPublicKeyIutX, newTc.StaticPublicKeyIutX, nameof(newTc.StaticPublicKeyIutX));
                Assert.AreEqual(tc.StaticPublicKeyIutY, newTc.StaticPublicKeyIutY, nameof(newTc.StaticPublicKeyIutY));
                Assert.AreEqual(tc.EphemeralPrivateKeyIut, newTc.EphemeralPrivateKeyIut, nameof(newTc.EphemeralPrivateKeyIut));
                Assert.AreEqual(tc.EphemeralPublicKeyIutX, newTc.EphemeralPublicKeyIutX, nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreEqual(tc.EphemeralPublicKeyIutY, newTc.EphemeralPublicKeyIutY, nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                Assert.AreNotEqual(tc.StaticPrivateKeyIut, newTc.StaticPrivateKeyIut, nameof(newTc.StaticPrivateKeyIut));
                Assert.AreNotEqual(tc.StaticPublicKeyIutX, newTc.StaticPublicKeyIutX, nameof(newTc.StaticPublicKeyIutX));
                Assert.AreNotEqual(tc.StaticPublicKeyIutY, newTc.StaticPublicKeyIutY, nameof(newTc.StaticPublicKeyIutY));
                Assert.AreNotEqual(tc.EphemeralPrivateKeyIut, newTc.EphemeralPrivateKeyIut, nameof(newTc.EphemeralPrivateKeyIut));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutX, newTc.EphemeralPublicKeyIutX, nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutY, newTc.EphemeralPublicKeyIutY, nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreNotEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreNotEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "val")
            {
                Assert.AreEqual(tc.OiLen, newTc.OiLen, nameof(newTc.OiLen));
                Assert.AreEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));
                Assert.AreEqual(tc.Tag, newTc.Tag, nameof(newTc.Tag));
            }

            if (testType == "aft")
            {
                Assert.AreNotEqual(tc.OiLen, newTc.OiLen, nameof(newTc.OiLen));
                Assert.AreNotEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));
                Assert.AreNotEqual(tc.Tag, newTc.Tag, nameof(newTc.Tag));
            }
            
            Assert.AreNotEqual(tc.Z, newTc.Z, nameof(newTc.Z));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
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

            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.StaticPublicKeyServerX, newTc.StaticPublicKeyServerX, nameof(newTc.StaticPublicKeyServerX));
            Assert.AreEqual(tc.StaticPublicKeyServerY, newTc.StaticPublicKeyServerY, nameof(newTc.StaticPublicKeyServerY));
            Assert.AreEqual(tc.EphemeralPublicKeyServerX, newTc.EphemeralPublicKeyServerX, nameof(newTc.EphemeralPublicKeyServerX));
            Assert.AreEqual(tc.EphemeralPublicKeyServerY, newTc.EphemeralPublicKeyServerY, nameof(newTc.EphemeralPublicKeyServerY));
            Assert.AreEqual(tc.DkmNonceServer, newTc.DkmNonceServer, nameof(newTc.DkmNonceServer));
            Assert.AreEqual(tc.EphemeralNonceServer, newTc.EphemeralNonceServer, nameof(newTc.EphemeralNonceServer));
            Assert.AreEqual(tc.NonceNoKc, newTc.NonceNoKc, nameof(newTc.NonceNoKc));

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.AreEqual(tc.NonceAesCcm, newTc.NonceAesCcm, nameof(newTc.NonceAesCcm));
            }
            else
            {
                Assert.AreNotEqual(tc.NonceAesCcm, newTc.NonceAesCcm, nameof(newTc.NonceAesCcm));
            }


            if (testType == "val")
            {
                Assert.AreEqual(tc.StaticPrivateKeyIut, newTc.StaticPrivateKeyIut, nameof(newTc.StaticPrivateKeyIut));
                Assert.AreEqual(tc.StaticPublicKeyIutX, newTc.StaticPublicKeyIutX, nameof(newTc.StaticPublicKeyIutX));
                Assert.AreEqual(tc.StaticPublicKeyIutY, newTc.StaticPublicKeyIutY, nameof(newTc.StaticPublicKeyIutY));
                Assert.AreEqual(tc.EphemeralPrivateKeyIut, newTc.EphemeralPrivateKeyIut, nameof(newTc.EphemeralPrivateKeyIut));
                Assert.AreEqual(tc.EphemeralPublicKeyIutX, newTc.EphemeralPublicKeyIutX, nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreEqual(tc.EphemeralPublicKeyIutY, newTc.EphemeralPublicKeyIutY, nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                Assert.AreNotEqual(tc.StaticPrivateKeyIut, newTc.StaticPrivateKeyIut, nameof(newTc.StaticPrivateKeyIut));
                Assert.AreNotEqual(tc.StaticPublicKeyIutX, newTc.StaticPublicKeyIutX, nameof(newTc.StaticPublicKeyIutX));
                Assert.AreNotEqual(tc.StaticPublicKeyIutY, newTc.StaticPublicKeyIutY, nameof(newTc.StaticPublicKeyIutY));
                Assert.AreNotEqual(tc.EphemeralPrivateKeyIut, newTc.EphemeralPrivateKeyIut, nameof(newTc.EphemeralPrivateKeyIut));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutX, newTc.EphemeralPublicKeyIutX, nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutY, newTc.EphemeralPublicKeyIutY, nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreNotEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreNotEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "val")
            {
                Assert.AreEqual(tc.OiLen, newTc.OiLen, nameof(newTc.OiLen));
                Assert.AreEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));
                Assert.AreEqual(tc.Tag, newTc.Tag, nameof(newTc.Tag));
            }

            if (testType == "aft")
            {
                Assert.AreNotEqual(tc.OiLen, newTc.OiLen, nameof(newTc.OiLen));
                Assert.AreNotEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));
                Assert.AreNotEqual(tc.Tag, newTc.Tag, nameof(newTc.Tag));
            }

            Assert.AreNotEqual(tc.Z, newTc.Z, nameof(newTc.Z));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
        }
    }
}