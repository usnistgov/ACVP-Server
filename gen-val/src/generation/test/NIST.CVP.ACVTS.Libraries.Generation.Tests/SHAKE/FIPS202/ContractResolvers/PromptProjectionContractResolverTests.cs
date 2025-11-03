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

            Assert.That(tg.TestGroupId, Is.EqualTo(newTg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(tg.TestType, Is.EqualTo(newTg.TestType), nameof(newTg.TestType));
            Assert.That(tg.Tests.Count, Is.EqualTo(newTg.Tests.Count), nameof(newTg.Tests));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("aft")]
        [TestCase("mct")]
        [TestCase("ldt")]
        public void ShouldSerializeCaseProperties(string testType)
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

            if (testType == "aft")
            {
                Assert.That(tc.Message, Is.EqualTo(newTc.Message), nameof(newTc.Message));
                Assert.That(tc.MessageLength, Is.EqualTo(newTc.MessageLength), nameof(newTc.MessageLength));
            }
            else if (testType == "mct")
            {
                Assert.That(tc.Message, Is.EqualTo(newTc.Message), nameof(newTc.Message));
                Assert.That(tc.MessageLength, Is.EqualTo(newTc.MessageLength), nameof(newTc.MessageLength));
            }
            
            Assert.That(newTc.Digest, Is.Null, nameof(newTc.Digest));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
