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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            if (testType == "aft")
            {
                Assert.AreEqual(tc.Message, newTc.Message, nameof(newTc.Message));
                Assert.AreEqual(tc.MessageLength, newTc.MessageLength, nameof(newTc.MessageLength));
            }
            else if (testType == "mct")
            {
                Assert.AreEqual(tc.Message, newTc.Message, nameof(newTc.Message));
                Assert.AreEqual(tc.MessageLength, newTc.MessageLength, nameof(newTc.MessageLength));
            }
            else if (testType == "ldt")
            {
                Assert.AreEqual(tc.LargeMessage.Content, newTc.LargeMessage.Content, nameof(newTc.LargeMessage.Content));
                Assert.AreEqual(tc.LargeMessage.ContentLength, newTc.LargeMessage.ContentLength, nameof(newTc.LargeMessage.ContentLength));
                Assert.AreEqual(tc.LargeMessage.ExpansionTechnique, newTc.LargeMessage.ExpansionTechnique, nameof(newTc.LargeMessage.ExpansionTechnique));
                Assert.AreEqual(tc.LargeMessage.FullLength, newTc.LargeMessage.FullLength, nameof(newTc.LargeMessage.FullLength));
            }

            Assert.IsNull(newTc.ResultsArray, nameof(newTc.ResultsArray));
            Assert.IsNull(newTc.Digest, nameof(newTc.Digest));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
