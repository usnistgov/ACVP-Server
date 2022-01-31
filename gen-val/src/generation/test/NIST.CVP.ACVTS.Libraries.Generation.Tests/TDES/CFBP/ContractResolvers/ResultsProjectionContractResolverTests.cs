using System;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFBP.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));

            Assert.AreNotEqual(tg.Function, newTg.Function, nameof(newTg.Function));
            Assert.AreNotEqual(tg.KeyingOption, newTg.KeyingOption, nameof(newTg.KeyingOption));
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText, results array (when mct)
        /// all other properties excluded
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

            if (tg.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
            {
                for (var i = 0; i < tc.ResultsArray.Count; i++)
                {
                    Assert.AreEqual(tc.ResultsArray[i].IV, newTc.ResultsArray[i].IV, "mctIv");
                    Assert.AreEqual(tc.ResultsArray[i].Key1, newTc.ResultsArray[i].Key1, "mctKey1");
                    Assert.AreEqual(tc.ResultsArray[i].Key2, newTc.ResultsArray[i].Key2, "mctKey2");
                    Assert.AreEqual(tc.ResultsArray[i].Key3, newTc.ResultsArray[i].Key3, "mctKey3");
                    Assert.AreEqual(tc.ResultsArray[i].CipherText, newTc.ResultsArray[i].CipherText, "mctCt");
                    Assert.AreEqual(tc.ResultsArray[i].PlainText, newTc.ResultsArray[i].PlainText, "mctPt");
                }
            }
            else
            {
                Assert.AreEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));
                Assert.AreEqual(tc.CipherText1, newTc.CipherText1, nameof(newTc.CipherText1));
                Assert.AreEqual(tc.CipherText2, newTc.CipherText2, nameof(newTc.CipherText2));
                Assert.AreEqual(tc.CipherText3, newTc.CipherText3, nameof(newTc.CipherText3));
                Assert.IsNull(newTc.ResultsArray, nameof(newTc.ResultsArray));
            }

            Assert.AreNotEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            Assert.AreNotEqual(tc.Key1, newTc.Key1, nameof(newTc.Key1));
            Assert.AreNotEqual(tc.Key2, newTc.Key2, nameof(newTc.Key2));
            Assert.AreNotEqual(tc.Key3, newTc.Key3, nameof(newTc.Key3));
            Assert.AreNotEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));
            Assert.AreNotEqual(tc.PlainText1, newTc.PlainText1, nameof(newTc.PlainText1));
            Assert.AreNotEqual(tc.PlainText2, newTc.PlainText2, nameof(newTc.PlainText2));
            Assert.AreNotEqual(tc.PlainText3, newTc.PlainText3, nameof(newTc.PlainText3));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }

        /// <summary>
        /// Decrypt test group should contain the plainText, results array (when mct)
        /// all other properties excluded
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

            if (tg.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
            {
                for (var i = 0; i < tc.ResultsArray.Count; i++)
                {
                    Assert.AreEqual(tc.ResultsArray[i].IV, newTc.ResultsArray[i].IV, "mctIv");
                    Assert.AreEqual(tc.ResultsArray[i].Key1, newTc.ResultsArray[i].Key1, "mctKey1");
                    Assert.AreEqual(tc.ResultsArray[i].Key2, newTc.ResultsArray[i].Key2, "mctKey2");
                    Assert.AreEqual(tc.ResultsArray[i].Key3, newTc.ResultsArray[i].Key3, "mctKey3");
                    Assert.AreEqual(tc.ResultsArray[i].CipherText, newTc.ResultsArray[i].CipherText, "mctCt");
                    Assert.AreEqual(tc.ResultsArray[i].PlainText, newTc.ResultsArray[i].PlainText, "mctPt");
                }
            }
            else
            {
                Assert.AreEqual(tc.PlainText, newTc.PlainText, nameof(newTc.PlainText));
                Assert.AreEqual(tc.PlainText1, newTc.PlainText1, nameof(newTc.PlainText1));
                Assert.AreEqual(tc.PlainText2, newTc.PlainText2, nameof(newTc.PlainText2));
                Assert.AreEqual(tc.PlainText3, newTc.PlainText3, nameof(newTc.PlainText3));
                Assert.IsNull(newTc.ResultsArray, nameof(newTc.ResultsArray));
            }

            Assert.AreNotEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            Assert.AreNotEqual(tc.Key1, newTc.Key1, nameof(newTc.Key1));
            Assert.AreNotEqual(tc.Key2, newTc.Key2, nameof(newTc.Key2));
            Assert.AreNotEqual(tc.Key3, newTc.Key3, nameof(newTc.Key3));
            Assert.AreNotEqual(tc.CipherText, newTc.CipherText, nameof(newTc.CipherText));
            Assert.AreNotEqual(tc.CipherText1, newTc.CipherText1, nameof(newTc.CipherText1));
            Assert.AreNotEqual(tc.CipherText2, newTc.CipherText2, nameof(newTc.CipherText2));
            Assert.AreNotEqual(tc.CipherText3, newTc.CipherText3, nameof(newTc.CipherText3));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
