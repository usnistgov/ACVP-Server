using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.ECC.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));

            Assert.AreNotEqual(tg.Scheme, newTg.Scheme, nameof(newTg.Scheme));
            Assert.AreNotEqual(tg.KasRole, newTg.KasRole, nameof(newTg.KasRole));
            Assert.AreNotEqual(tg.ParmSet, newTg.ParmSet, nameof(newTg.ParmSet));
            Assert.AreNotEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));
            Assert.AreNotEqual(tg.Curve, newTg.Curve, nameof(newTg.Curve));

            Assert.IsFalse(json.ToLower().Contains(nameof(TestGroup.KasMode).ToLower()), nameof(TestGroup.KasMode));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            if (testType == "val")
            {
                Assert.AreEqual(tc.TestPassed, newTc.TestPassed, nameof(newTc.TestPassed));

                Assert.AreNotEqual(tc.StaticPublicKeyIutX, newTc.StaticPublicKeyIutX, nameof(newTc.StaticPublicKeyIutX));
                Assert.AreNotEqual(tc.StaticPublicKeyIutY, newTc.StaticPublicKeyIutY, nameof(newTc.StaticPublicKeyIutY));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutX, newTc.EphemeralPublicKeyIutX, nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreNotEqual(tc.EphemeralPublicKeyIutY, newTc.EphemeralPublicKeyIutY, nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreNotEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreNotEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            if (testType == "aft")
            {
                // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
                Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.IsTrue(regex.Matches(json).Count == 0);

                Assert.AreEqual(tc.StaticPublicKeyIutX.ToPositiveBigInteger(), newTc.StaticPublicKeyIutX.ToPositiveBigInteger(), nameof(newTc.StaticPublicKeyIutX));
                Assert.AreEqual(tc.StaticPublicKeyIutY.ToPositiveBigInteger(), newTc.StaticPublicKeyIutY.ToPositiveBigInteger(), nameof(newTc.StaticPublicKeyIutY));
                Assert.AreEqual(tc.EphemeralPublicKeyIutX.ToPositiveBigInteger(), newTc.EphemeralPublicKeyIutX.ToPositiveBigInteger(), nameof(newTc.EphemeralPublicKeyIutX));
                Assert.AreEqual(tc.EphemeralPublicKeyIutY.ToPositiveBigInteger(), newTc.EphemeralPublicKeyIutY.ToPositiveBigInteger(), nameof(newTc.EphemeralPublicKeyIutY));
                Assert.AreEqual(tc.DkmNonceIut, newTc.DkmNonceIut, nameof(newTc.DkmNonceIut));
                Assert.AreEqual(tc.EphemeralNonceIut, newTc.EphemeralNonceIut, nameof(newTc.EphemeralNonceIut));
            }

            Assert.IsNull(newTc.StaticPrivateKeyIut, nameof(newTc.StaticPrivateKeyIut));
            Assert.IsNull(newTc.EphemeralPrivateKeyIut, nameof(newTc.EphemeralPrivateKeyIut));


            Assert.AreNotEqual(tc.NonceAesCcm, newTc.NonceAesCcm, nameof(newTc.NonceAesCcm));

            Assert.AreNotEqual(tc.OiLen, newTc.OiLen, nameof(newTc.OiLen));
            Assert.AreNotEqual(tc.OtherInfo, newTc.OtherInfo, nameof(newTc.OtherInfo));
            Assert.AreNotEqual(tc.Tag, newTc.Tag, nameof(newTc.Tag));

            if (testType == "aft")
            {
                Assert.AreEqual(tc.HashZ, newTc.HashZ, nameof(newTc.HashZ));
            }
            else
            {
                Assert.AreNotEqual(tc.HashZ, newTc.HashZ, nameof(newTc.HashZ));
            }

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);
        }
    }
}
