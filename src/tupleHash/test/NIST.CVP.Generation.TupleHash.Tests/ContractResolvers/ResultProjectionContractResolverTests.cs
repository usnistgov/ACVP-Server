using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.TupleHash.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests.ContractResolvers
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
            Assert.AreNotEqual(tg.BitOrientedInput, newTg.BitOrientedInput, nameof(newTg.BitOrientedInput));
            Assert.AreNotEqual(tg.DigestSize, newTg.DigestSize, nameof(newTg.DigestSize));
            Assert.AreNotEqual(tg.BitOrientedOutput, newTg.BitOrientedOutput, nameof(newTg.BitOrientedOutput));
            Assert.AreNotEqual(tg.IncludeNull, newTg.IncludeNull, nameof(newTg.IncludeNull));
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText, results array (when mct)
        /// all other properties excluded
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("tuplehash", "aft")]
        [TestCase("tuplehash", "mct")]
        public void ShouldSerializeProperties(string function, string testType)
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
            Assert.AreEqual(tc.Digest, newTc.Digest, nameof(newTc.Digest));
            Assert.AreEqual(tc.DigestLength, newTc.DigestLength, nameof(newTc.DigestLength));

            if (tg.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
            {
                for (var i = 0; i < tc.ResultsArray.Count; i++)
                {
                    Assert.AreEqual(tc.ResultsArray[i].Digest, newTc.ResultsArray[i].Digest, "mctDigest");
                    Assert.AreEqual(tc.ResultsArray[i].Tuple, newTc.ResultsArray[i].Tuple, "mctTuple");
                    Assert.AreEqual(tc.ResultsArray[i].Customization, newTc.ResultsArray[i].Customization, "mctCustomization");
                }
            }
            else
            {
                Assert.IsNull(newTc.ResultsArray, nameof(newTc.ResultsArray));
            }

            Assert.AreNotEqual(tc.Tuple, newTc.Tuple, nameof(newTc.Tuple));
            Assert.AreNotEqual(tc.MessageLength, newTc.MessageLength, nameof(newTc.MessageLength));
            Assert.AreNotEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
