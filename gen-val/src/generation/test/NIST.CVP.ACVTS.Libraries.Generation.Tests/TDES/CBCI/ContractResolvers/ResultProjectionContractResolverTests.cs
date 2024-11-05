using System;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBCI.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBCI.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CBCI.ContractResolvers
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

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

            Assert.That(newTg.Function, Is.Not.EqualTo(tg.Function), nameof(newTg.Function));
            Assert.That(newTg.KeyingOption, Is.Not.EqualTo(tg.KeyingOption), nameof(newTg.KeyingOption));
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

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (tg.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
            {
                for (var i = 0; i < tc.ResultsArray.Count; i++)
                {
                    Assert.That(newTc.ResultsArray[i].IV, Is.EqualTo(tc.ResultsArray[i].IV), "mctIv");
                    Assert.That(newTc.ResultsArray[i].Key1, Is.EqualTo(tc.ResultsArray[i].Key1), "mctKey1");
                    Assert.That(newTc.ResultsArray[i].Key2, Is.EqualTo(tc.ResultsArray[i].Key2), "mctKey2");
                    Assert.That(newTc.ResultsArray[i].Key3, Is.EqualTo(tc.ResultsArray[i].Key3), "mctKey3");
                    Assert.That(newTc.ResultsArray[i].CipherText, Is.EqualTo(tc.ResultsArray[i].CipherText), "mctCt");
                    Assert.That(newTc.ResultsArray[i].PlainText, Is.EqualTo(tc.ResultsArray[i].PlainText), "mctPt");
                }
            }
            else
            {
                Assert.That(newTc.CipherText, Is.EqualTo(tc.CipherText), nameof(newTc.CipherText));
                Assert.That(newTc.ResultsArray, Is.Null, nameof(newTc.ResultsArray));
            }

            Assert.That(newTc.Iv, Is.Not.EqualTo(tc.Iv), nameof(newTc.Iv));
            Assert.That(newTc.Key1, Is.Not.EqualTo(tc.Key1), nameof(newTc.Key1));
            Assert.That(newTc.Key2, Is.Not.EqualTo(tc.Key2), nameof(newTc.Key2));
            Assert.That(newTc.Key3, Is.Not.EqualTo(tc.Key3), nameof(newTc.Key3));
            Assert.That(newTc.PlainText, Is.Not.EqualTo(tc.PlainText), nameof(newTc.PlainText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
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

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (tg.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
            {
                for (var i = 0; i < tc.ResultsArray.Count; i++)
                {
                    Assert.That(newTc.ResultsArray[i].IV, Is.EqualTo(tc.ResultsArray[i].IV), "mctIv");
                    Assert.That(newTc.ResultsArray[i].Key1, Is.EqualTo(tc.ResultsArray[i].Key1), "mctKey1");
                    Assert.That(newTc.ResultsArray[i].Key2, Is.EqualTo(tc.ResultsArray[i].Key2), "mctKey2");
                    Assert.That(newTc.ResultsArray[i].Key3, Is.EqualTo(tc.ResultsArray[i].Key3), "mctKey3");
                    Assert.That(newTc.ResultsArray[i].CipherText, Is.EqualTo(tc.ResultsArray[i].CipherText), "mctCt");
                    Assert.That(newTc.ResultsArray[i].PlainText, Is.EqualTo(tc.ResultsArray[i].PlainText), "mctPt");
                }
            }
            else
            {
                Assert.That(newTc.PlainText, Is.EqualTo(tc.PlainText), nameof(newTc.PlainText));
                Assert.That(newTc.ResultsArray, Is.Null, nameof(newTc.ResultsArray));
            }

            Assert.That(newTc.Iv, Is.Not.EqualTo(tc.Iv), nameof(newTc.Iv));
            Assert.That(newTc.Key1, Is.Not.EqualTo(tc.Key1), nameof(newTc.Key1));
            Assert.That(newTc.Key2, Is.Not.EqualTo(tc.Key2), nameof(newTc.Key2));
            Assert.That(newTc.Key3, Is.Not.EqualTo(tc.Key3), nameof(newTc.Key3));
            Assert.That(newTc.CipherText, Is.Not.EqualTo(tc.CipherText), nameof(newTc.CipherText));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
