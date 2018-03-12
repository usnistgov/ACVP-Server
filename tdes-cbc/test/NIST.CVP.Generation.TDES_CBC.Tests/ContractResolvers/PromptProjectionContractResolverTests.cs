using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.TDES_CBC.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Function, newTg.Function, nameof(newTg.Function));
            Assert.AreEqual(tg.KeyingOption, newTg.KeyingOption, nameof(newTg.KeyingOption));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("encrypt", "aft")]
        [TestCase("encrypt", "mct")]
        public void ShouldSerializeEncryptCaseProperties(string function, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, testType);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Iv, newTc.Iv, nameof(newTc.Iv));
            Assert.AreEqual(tc.Key1, newTc.Key1, nameof(newTc.Key1));
            Assert.AreEqual(tc.Key2, newTc.Key2, nameof(newTc.Key2));
            Assert.AreEqual(tc.Key3, newTc.Key3, nameof(newTc.Key3));
            Assert.AreEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));

            Assert.IsNull(newTc.ResultsArray, nameof(newTc.ResultsArray));

            Assert.AreNotEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }

        /// <summary>
        /// Decrypt test group should not contain the plainText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("decrypt", "aft")]
        [TestCase("decrypt", "mct")]
        public void ShouldSerializeDecryptCaseProperties(string function, string testType)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, testType);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Iv, newTc.Iv, nameof(newTc.Iv));
            Assert.AreEqual(tc.Key1, newTc.Key1, nameof(newTc.Key1));
            Assert.AreEqual(tc.Key2, newTc.Key2, nameof(newTc.Key2));
            Assert.AreEqual(tc.Key3, newTc.Key3, nameof(newTc.Key3));
            Assert.AreEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));

            Assert.IsNull(newTc.ResultsArray, nameof(newTc.ResultsArray));

            Assert.AreNotEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
