using System.Text.RegularExpressions;
using NIST.CVP.Generation.AES_XTS.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XTS.Tests.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", "hex");
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Direction, newTg.Direction, nameof(newTg.Direction));
            Assert.AreEqual(tg.PayloadLen, newTg.PayloadLen, nameof(newTg.PayloadLen));
            Assert.AreEqual(tg.TweakMode, newTg.TweakMode, nameof(newTg.TweakMode));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="tweakMode">The tweakmode used (hex/number)</param>
        [Test]
        [TestCase("encrypt", "hex")]
        [TestCase("encrypt", "number")]
        public void ShouldSerializeEncryptCaseProperties(string function, string tweakMode)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, tweakMode);
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

            Assert.AreNotEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            if (tweakMode == "hex")
            {
                Assert.AreEqual(tc.I, newTc.I, nameof(newTc.I));
                Assert.AreEqual(0, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
            }
            else
            {
                Assert.IsNull(newTc.I, nameof(newTc.I));
            }

            if (tweakMode == "number")
            {
                Assert.AreEqual(tc.SequenceNumber, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
                Assert.IsNull(newTc.I, nameof(tc.I));
            }
            else
            {
                Assert.AreEqual(0, tc.SequenceNumber, nameof(tc.SequenceNumber));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);
        }

        /// <summary>
        /// Decrypt test group should not contain the plainText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="tweakMode">The tweakmode used (hex/number)</param>
        [Test]
        [TestCase("decrypt", "hex")]
        [TestCase("decrypt", "number")]
        public void ShouldSerializeDecryptCaseProperties(string function, string tweakMode)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, tweakMode);
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

            Assert.AreNotEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            if (tweakMode == "hex")
            {
                Assert.AreEqual(tc.I, newTc.I, nameof(newTc.I));
                Assert.AreEqual(0, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
            }
            else
            {
                Assert.IsNull(newTc.I, nameof(newTc.I));
            }

            if (tweakMode == "number")
            {
                Assert.AreEqual(tc.SequenceNumber, newTc.SequenceNumber, nameof(newTc.SequenceNumber));
                Assert.IsNull(newTc.I, nameof(tc.I));
            }
            else
            {
                Assert.AreEqual(0, tc.SequenceNumber, nameof(tc.SequenceNumber));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);
        }
    }
}