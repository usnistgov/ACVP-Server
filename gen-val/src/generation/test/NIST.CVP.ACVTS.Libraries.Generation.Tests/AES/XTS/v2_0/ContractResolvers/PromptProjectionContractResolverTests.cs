using System;
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

        /// <summary>
        /// All group level properties are present in the prompt file
        /// </summary>
        [Test]
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, true, BlockCipherDirections.Encrypt, XtsTweakModes.Hex);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Direction, newTg.Direction, nameof(newTg.Direction));
            Assert.AreEqual(tg.TweakMode, newTg.TweakMode, nameof(newTg.TweakMode));

            Assert.IsNull(newTg.DataUnitLen, nameof(newTg.DataUnitLen));
            Assert.IsNull(newTg.PayloadLen, nameof(newTg.PayloadLen));        // Group PayloadLen is a domain based off the parameters
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));
            Assert.AreEqual(tc.DataUnitLen, newTc.DataUnitLen, nameof(newTc.DataUnitLen));

            Assert.AreNotEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            switch (tweakMode)
            {
                case XtsTweakModes.Hex:
                    Assert.AreEqual(tc.I, newTc.I, nameof(newTc.I));
                    Assert.AreEqual(0, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
                    break;
                case XtsTweakModes.Number:
                    Assert.AreEqual(tc.SequenceNumber, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
                    Assert.IsNull(newTc.I, nameof(tc.I));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tweakMode), tweakMode, null);
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));
            Assert.AreEqual(tc.DataUnitLen, newTc.DataUnitLen, nameof(newTc.DataUnitLen));

            Assert.AreNotEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            switch (tweakMode)
            {
                case XtsTweakModes.Hex:
                    Assert.AreEqual(tc.I, newTc.I, nameof(newTc.I));
                    Assert.AreEqual(0, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
                    break;
                case XtsTweakModes.Number:
                    Assert.AreEqual(tc.SequenceNumber, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
                    Assert.IsNull(newTc.I, nameof(tc.I));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tweakMode), tweakMode, null);
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);
        }
    }
}
