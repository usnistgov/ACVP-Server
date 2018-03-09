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
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1, "encrypt", true, true);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));

            Assert.AreNotEqual(tg.Function, newTg.Function, nameof(newTg.Function));
            Assert.AreNotEqual(tg.KeyLength, newTg.KeyLength, nameof(newTg.KeyLength));
            Assert.AreNotEqual(tg.AADLength, newTg.AADLength, nameof(newTg.AADLength));
            Assert.AreNotEqual(tg.IVLength, newTg.IVLength, nameof(newTg.IVLength));
            Assert.AreNotEqual(tg.TagLength, newTg.TagLength, nameof(newTg.TagLength));
            Assert.AreNotEqual(tg.PTLength, newTg.PTLength, nameof(newTg.PTLength));
            Assert.AreNotEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText
        /// all other properties excluded
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="deferred">Is the test deferred? (internal IV)</param>
        [Test]
        [TestCase("encrypt", true)]
        [TestCase("encrypt", false)]
        public void ShouldSerializeEncryptCaseProperties(string function, bool deferred)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, deferred, true);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            
            // not included in results file
            Assert.AreNotEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreNotEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));

            if (deferred)
            {
                Assert.IsNotNull(newTc.CipherText, nameof(newTc.CipherText));
                Assert.IsNotNull(newTc.Tag, nameof(newTc.Tag));
                Assert.IsNotNull(newTc.IV, nameof(newTc.IV));
            }
            else
            {
                Assert.IsNull(newTc.IV, nameof(newTc.IV));
                Assert.AreEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));
                Assert.AreEqual(tc.Tag, newTc.Tag, nameof(newTc.Tag));
            }

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }

        /// <summary>
        /// Decrypt test group should contain the plainText, results array (when mct)
        /// all other properties excluded
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

            if (testPassed)
            {
                Assert.AreEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));
                Regex regexPass = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.IsTrue(regexPass.Matches(json).Count == 0, nameof(regexPass));
            }
            else
            {
                Assert.AreEqual(tc.TestPassed, newTc.TestPassed, nameof(newTc.TestPassed));

                Regex regexFail = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.IsTrue(regexFail.Matches(json).Count > 0, nameof(regexFail));
            }

            // not included in results file
            Assert.AreNotEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            Assert.AreNotEqual(tc.Key, newTc.Key, nameof(newTc.Key));
            Assert.AreNotEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexDeferred.Matches(json).Count == 0);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            if (testPassed)
            {
                Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
                Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
            }
            else
            {
                Assert.AreEqual(tc.TestPassed, newTc.TestPassed, nameof(newTc.TestPassed));
                Assert.IsFalse(newTc.TestPassed, nameof(newTc.TestPassed));
            }
            
        }
    }
}