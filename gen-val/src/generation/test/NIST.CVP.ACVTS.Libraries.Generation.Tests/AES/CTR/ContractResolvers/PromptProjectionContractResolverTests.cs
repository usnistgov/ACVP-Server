using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CTR.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.Direction, Is.EqualTo(tg.Direction), nameof(newTg.Direction));
            Assert.That(newTg.KeyLength, Is.EqualTo(tg.KeyLength), nameof(newTg.KeyLength));
            Assert.That(tg.IvGenMode, Is.EqualTo(IvGenModes.None), nameof(newTg.KeyLength));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("encrypt", "aft")]
        [TestCase("encrypt", "counter")]
        public void ShouldSerializeEncryptCaseProperties(string function, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, testType);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.IV, Is.EqualTo(tc.IV), nameof(newTc.IV));
            Assert.That(newTc.PlainText, Is.EqualTo(tc.PlainText), nameof(newTc.PlainText));

            Assert.That(newTc.CipherText, Is.Not.EqualTo(tc.CipherText), nameof(newTc.CipherText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        /// <summary>
        /// Decrypt test group should not contain the plainText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("decrypt", "aft")]
        [TestCase("decrypt", "counter")]
        public void ShouldSerializeDecryptCaseProperties(string function, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, testType);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.IV, Is.EqualTo(tc.IV), nameof(newTc.IV));
            Assert.That(newTc.CipherText, Is.EqualTo(tc.CipherText), nameof(newTc.CipherText));

            Assert.That(newTc.PlainText, Is.Not.EqualTo(tc.PlainText), nameof(newTc.PlainText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        [Test]
        public void ShouldSerializeRfcInternalEncryptCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", "AFT");
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            tg.RfcTestMode = true;
            tg.IvGenMode = IvGenModes.Internal;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.IV, Is.Null, nameof(newTc.IV));
            Assert.That(newTc.PlainText, Is.EqualTo(tc.PlainText), nameof(newTc.PlainText));

            Assert.That(newTc.CipherText, Is.Null, nameof(newTc.CipherText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        [Test]
        public void ShouldSerializeRfcInternalDecryptCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, "decrypt", "AFT");
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            tg.RfcTestMode = true;
            tg.IvGenMode = IvGenModes.Internal;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.IV, Is.EqualTo(tc.IV), nameof(newTc.IV));
            Assert.That(newTc.PlainText, Is.Null, nameof(newTc.PlainText));

            Assert.That(newTc.CipherText, Is.EqualTo(tc.CipherText), nameof(newTc.CipherText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        [Test]
        public void ShouldSerializeRfcExternalEncryptCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", "AFT");
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            tg.RfcTestMode = true;
            tg.IvGenMode = IvGenModes.External;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.IV, Is.EqualTo(tc.IV), nameof(newTc.IV));
            Assert.That(newTc.PlainText, Is.EqualTo(tc.PlainText), nameof(newTc.PlainText));

            Assert.That(newTc.CipherText, Is.Null, nameof(newTc.CipherText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        [Test]
        public void ShouldSerializeRfcExternalDecryptCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, "decrypt", "AFT");
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            tg.RfcTestMode = true;
            tg.IvGenMode = IvGenModes.External;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Key, Is.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.IV, Is.EqualTo(tc.IV), nameof(newTc.IV));
            Assert.That(newTc.PlainText, Is.Null, nameof(newTc.PlainText));

            Assert.That(newTc.CipherText, Is.EqualTo(tc.CipherText), nameof(newTc.CipherText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
