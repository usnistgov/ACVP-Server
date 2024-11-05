using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v2_0.ContractResolvers
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

        [Test]
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, true, BlockCipherDirections.Encrypt, XtsTweakModes.Hex);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

            Assert.That(newTg.Direction, Is.EqualTo(default(BlockCipherDirections)), nameof(newTg.Direction));
            Assert.That(newTg.TweakMode, Is.EqualTo(default(XtsTweakModes)), nameof(newTg.TweakMode));
            Assert.That(newTg.PayloadLen, Is.Not.EqualTo(tg.PayloadLen), nameof(newTg.PayloadLen));
        }

        [Test]
        [TestCase(BlockCipherDirections.Encrypt, XtsTweakModes.Hex, true)]
        [TestCase(BlockCipherDirections.Encrypt, XtsTweakModes.Hex, false)]
        [TestCase(BlockCipherDirections.Encrypt, XtsTweakModes.Number, true)]
        [TestCase(BlockCipherDirections.Encrypt, XtsTweakModes.Number, false)]
        public void ShouldSerializeEncryptCaseProperties(BlockCipherDirections function, XtsTweakModes tweakMode, bool payloadLenMatch)
        {
            var tvs = TestDataMother.GetTestGroups(1, payloadLenMatch, function, tweakMode);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.CipherText, Is.EqualTo(tc.CipherText), nameof(newTc.CipherText));

            Assert.That(newTc.Key, Is.Not.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.PlainText, Is.Not.EqualTo(tc.PlainText), nameof(newTc.PlainText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));
            Assert.That(newTc.DataUnitLen, Is.Not.EqualTo(tc.DataUnitLen), nameof(newTc.DataUnitLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);
        }

        [Test]
        [TestCase(BlockCipherDirections.Decrypt, XtsTweakModes.Hex, true)]
        [TestCase(BlockCipherDirections.Decrypt, XtsTweakModes.Hex, false)]
        [TestCase(BlockCipherDirections.Decrypt, XtsTweakModes.Number, true)]
        [TestCase(BlockCipherDirections.Decrypt, XtsTweakModes.Number, false)]
        public void ShouldSerializeDecryptCaseProperties(BlockCipherDirections function, XtsTweakModes tweakMode, bool payloadLenMatch)
        {
            var tvs = TestDataMother.GetTestGroups(1, true, function, tweakMode);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.PlainText, Is.EqualTo(tc.PlainText), nameof(newTc.PlainText));

            Assert.That(newTc.Key, Is.Not.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.CipherText, Is.Not.EqualTo(tc.CipherText), nameof(newTc.CipherText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));
            Assert.That(newTc.DataUnitLen, Is.Not.EqualTo(tc.DataUnitLen), nameof(newTc.DataUnitLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);
        }
    }
}
