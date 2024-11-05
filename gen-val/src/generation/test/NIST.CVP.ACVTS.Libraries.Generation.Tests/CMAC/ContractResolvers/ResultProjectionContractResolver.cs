using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups(1, "gen", true);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

            Assert.That(newTg.Function, Is.Not.EqualTo(tg.Function), nameof(newTg.Function));
            Assert.That(newTg.KeyLength, Is.Not.EqualTo(tg.KeyLength), nameof(newTg.KeyLength));
            Assert.That(newTg.MessageLength, Is.Not.EqualTo(tg.MessageLength), nameof(newTg.MessageLength));
            Assert.That(newTg.MacLength, Is.Not.EqualTo(tg.MacLength), nameof(newTg.MacLength));
        }

        /// <summary>
        /// Mac included
        /// </summary>
        /// <param name="function">The function being tested</param>
        [Test]
        [TestCase("gen")]
        public void ShouldSerializeEncryptCaseProperties(string function)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, true);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Mac, Is.EqualTo(tc.Mac), nameof(newTc.Mac));

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        /// <summary>
        /// TestPassed included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testPassed">The testType</param>
        [Test]
        [TestCase("ver", true)]
        [TestCase("ver", false)]
        public void ShouldSerializeDecryptCaseProperties(string function, bool testPassed)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, testPassed);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            Assert.That(newTc.TestPassed, Is.EqualTo(testPassed), nameof(newTc.TestPassed));

            // not included in results file
            Assert.That(newTc.Key, Is.Not.EqualTo(tc.Key), nameof(newTc.Key));
            Assert.That(newTc.Message, Is.Not.EqualTo(tc.Message), nameof(newTc.Message));
            Assert.That(newTc.Mac, Is.Not.EqualTo(tc.Mac), nameof(newTc.Mac));

            Regex regexDeferred = new Regex(nameof(TestCase.Deferred), RegexOptions.IgnoreCase);
            Assert.That(regexDeferred.Matches(json).Count == 0, Is.True);

            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regexTestPassed.Matches(json).Count > 0, Is.True);
        }
    }
}
