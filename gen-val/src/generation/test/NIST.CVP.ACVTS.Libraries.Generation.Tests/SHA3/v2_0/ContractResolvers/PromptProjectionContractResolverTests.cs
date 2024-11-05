using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v2_0.ContractResolvers
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

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
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

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (testType == "aft")
            {
                Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
                Assert.That(newTc.MessageLength, Is.EqualTo(tc.MessageLength), nameof(newTc.MessageLength));
            }
            else if (testType == "mct")
            {
                Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
                Assert.That(newTc.MessageLength, Is.EqualTo(tc.MessageLength), nameof(newTc.MessageLength));
            }
            else if (testType == "ldt")
            {
                Assert.That(newTc.LargeMessage.Content, Is.EqualTo(tc.LargeMessage.Content), nameof(newTc.LargeMessage.Content));
                Assert.That(newTc.LargeMessage.ContentLength, Is.EqualTo(tc.LargeMessage.ContentLength), nameof(newTc.LargeMessage.ContentLength));
                Assert.That(newTc.LargeMessage.ExpansionTechnique, Is.EqualTo(tc.LargeMessage.ExpansionTechnique), nameof(newTc.LargeMessage.ExpansionTechnique));
                Assert.That(newTc.LargeMessage.FullLength, Is.EqualTo(tc.LargeMessage.FullLength), nameof(newTc.LargeMessage.FullLength));
            }

            Assert.That(newTc.ResultsArray, Is.Null, nameof(newTc.ResultsArray));
            Assert.That(newTc.Digest, Is.Null, nameof(newTc.Digest));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
