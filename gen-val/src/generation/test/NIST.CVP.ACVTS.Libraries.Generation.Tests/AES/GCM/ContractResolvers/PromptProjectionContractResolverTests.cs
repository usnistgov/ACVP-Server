using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.GCM.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", "external", true, true);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Function, newTg.Function, nameof(newTg.Function));
            Assert.AreEqual(tg.KeyLength, newTg.KeyLength, nameof(newTg.KeyLength));
            Assert.AreEqual(tg.AadLength, newTg.AadLength, nameof(newTg.AadLength));
            Assert.AreEqual(tg.IvLength, newTg.IvLength, nameof(newTg.IvLength));
            Assert.AreEqual(tg.TagLength, newTg.TagLength, nameof(newTg.TagLength));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="ivGen">IV generation (internal/external from perspective of IUT)</param>
        /// <param name="deferred">Is this a deferred test? (Internal IV generation encrypt)</param>
        [Test]
        [TestCase("encrypt", "external", false)]
        [TestCase("encrypt", "internal", true)]
        public void ShouldSerializeEncryptCaseProperties(string function, string ivGen, bool deferred)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, ivGen, deferred, true);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreEqual(tc.AAD, newTc.AAD, nameof(newTc.AAD));
            Assert.AreEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));

            // Should never send in prompt file for encrypt CT or tag
            Assert.IsNull(newTc.CipherText, nameof(newTc.CipherText));
            Assert.IsNull(newTc.Tag, nameof(newTc.Tag));

            // If deferred, newTc.CT/Tag
            if (deferred)
            {
                Assert.IsNull(newTc.CipherText, nameof(newTc.CipherText));
                Assert.IsNull(newTc.Tag, nameof(newTc.Tag));
            }
            // when not deferred, TC should contain CT/Tag/IV, but only include the IV in the newCt
            else
            {
                Assert.IsNotNull(tc.CipherText, nameof(tc.CipherText));
                Assert.IsNotNull(tc.Tag, nameof(tc.Tag));
                Assert.IsNotNull(tc.IV, nameof(tc.IV));
                Assert.IsNotNull(newTc.IV, nameof(newTc.IV));

                Assert.AreEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            }

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
        }

        /// <summary>
        /// Decrypt test group should not contain the plainText, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testPassed">The testType</param>
        [Test]
        [TestCase("decrypt", true)]
        [TestCase("decrypt", false)]
        public void ShouldSerializeDecryptCaseProperties(string function, bool testPassed)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, "external", false, testPassed);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            Assert.AreEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreEqual(tc.AAD, newTc.AAD, nameof(newTc.AAD));
            Assert.AreEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));

            Assert.AreNotEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }


        [Test]
        public void ShouldSerializeGroupPropertiesGmac()
        {
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", "external", true, true);
            var tg = tvs.TestGroups[0];
            tg.AlgoMode = AlgoMode.AES_GMAC_v1_0;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Function, newTg.Function, nameof(newTg.Function));
            Assert.AreEqual(tg.KeyLength, newTg.KeyLength, nameof(newTg.KeyLength));
            Assert.AreEqual(tg.AadLength, newTg.AadLength, nameof(newTg.AadLength));
            Assert.AreEqual(tg.IvLength, newTg.IvLength, nameof(newTg.IvLength));
            Assert.AreEqual(tg.TagLength, newTg.TagLength, nameof(newTg.TagLength));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
        }

        [Test]
        [TestCase("encrypt", "external", false)]
        [TestCase("encrypt", "internal", true)]
        public void ShouldSerializeEncryptCasePropertiesGmac(string function, string ivGen, bool deferred)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, ivGen, deferred, true);
            var tg = tvs.TestGroups[0];
            tg.AlgoMode = AlgoMode.AES_GMAC_v1_0;
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreEqual(tc.AAD, newTc.AAD, nameof(newTc.AAD));

            // Should never send in prompt file for encrypt PT, CT, or tag
            Assert.IsNull(newTc.PlainText, nameof(newTc.PlainText));
            Assert.IsNull(newTc.CipherText, nameof(newTc.CipherText));
            Assert.IsNull(newTc.Tag, nameof(newTc.Tag));

            // If deferred, newTc.CT/Tag
            if (deferred)
            {
                Assert.IsNull(newTc.Tag, nameof(newTc.Tag));
            }
            // when not deferred, TC should contain Tag/IV, but only include the IV in the newCt
            else
            {
                Assert.IsNotNull(tc.Tag, nameof(tc.Tag));
                Assert.IsNotNull(tc.IV, nameof(tc.IV));
                Assert.IsNotNull(newTc.IV, nameof(newTc.IV));

                Assert.AreEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            }

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
        }

        [Test]
        [TestCase("decrypt", true)]
        [TestCase("decrypt", false)]
        public void ShouldSerializeDecryptCasePropertiesGmac(string function, bool testPassed)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, "external", false, testPassed);
            var tg = tvs.TestGroups[0];
            tg.AlgoMode = AlgoMode.AES_GMAC_v1_0;
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            Assert.AreEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreEqual(tc.AAD, newTc.AAD, nameof(newTc.AAD));
            Assert.IsNull(newTc.CipherText, nameof(newTc.CipherText));

            Assert.IsNull(newTc.PlainText, nameof(newTc.PlainText));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }

        [Test]
        [TestCase("encrypt", "internal", false)]
        [TestCase("encrypt", "external", true)]
        [TestCase("decrypt", "internal", true)]
        [TestCase("decrypt", "external", true)]
        public void ShouldIncludeIvInSpecificScenarios(string direction, string ivGeneration, bool shouldIvBeIncluded)
        {
            var tvs = TestDataMother.GetTestGroups(1, direction, ivGeneration, false, true);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            if (shouldIvBeIncluded)
            {
                Assert.AreEqual(tc.IV, newTc.IV);
            }
            else
            {
                Assert.AreNotEqual(tc.IV, newTc.IV);
            }
        }
    }
}
