using System.Text.RegularExpressions;
using NIST.CVP.Generation.AES_GCM.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", true, true);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Function, newTg.Function, nameof(newTg.Function));
            Assert.AreEqual(tg.KeyLength, newTg.KeyLength, nameof(newTg.KeyLength));
            Assert.AreEqual(tg.AADLength, newTg.AADLength, nameof(newTg.AADLength));
            Assert.AreEqual(tg.IVLength, newTg.IVLength, nameof(newTg.IVLength));
            Assert.AreEqual(tg.TagLength, newTg.TagLength, nameof(newTg.TagLength));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="deferred">Is this a deferred test? (Internal IV generation encrypt)</param>
        /// <param name="testPassed">Is this a pass test?</param>
        [Test]
        [TestCase("encrypt", false, true)]
        [TestCase("encrypt", true, true)]
        public void ShouldSerializeEncryptCaseProperties(string function, bool deferred, bool testPassed)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, deferred, testPassed);
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
            var tvs = TestDataMother.GetTestGroups(1, function, false, testPassed);
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
    }
}