using System;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202;
using NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHAKE.FIPS202.ContractResolvers
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
            
            Assert.That(tg.TestGroupId, Is.EqualTo(newTg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(tg.Tests.Count, Is.EqualTo(newTg.Tests.Count), nameof(newTg.Tests));

            Assert.That(tg.HashFunction, Is.Not.EqualTo(newTg.HashFunction), nameof(newTg.HashFunction));
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText, results array (when mct)
        /// all other properties excluded
        /// </summary>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("aft")]
        public void ShouldSerializeShakeCaseProperties(string testType)
        {
            var tvs = TestDataMother.GetTestGroups(1, testType);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(tc.ParentGroup.TestGroupId, Is.EqualTo(newTc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(tc.TestCaseId, Is.EqualTo(newTc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(tc.Digest, Is.EqualTo(newTc.Digest), nameof(newTc.Digest));

            Assert.That(tc.Message, Is.Not.EqualTo(newTc.Message), nameof(newTc.Message));
            Assert.That(tc.MessageLength, Is.Not.EqualTo(newTc.MessageLength), nameof(newTc.MessageLength));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
