using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.cSHAKE.ContractResolvers
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
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.HexCustomization, Is.EqualTo(tg.HexCustomization), nameof(newTg.HexCustomization));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
        [Test]
        [TestCase("cshake", "aft", true)]
        [TestCase("cshake", "aft", false)]
        [TestCase("cshake", "mct", true)]
        [TestCase("cshake", "mct", false)]
        public void ShouldSerializeCaseProperties(string function, string testType, bool hexCustomization)
        {
            var tvs = TestDataMother.GetTestGroups(1, function, testType, hexCustomization);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
            Assert.That(newTc.MessageLength, Is.EqualTo(tc.MessageLength), nameof(newTc.MessageLength));

            Assert.That(newTc.ResultsArray, Is.Null, nameof(newTc.ResultsArray));

            Assert.That(newTc.Digest, Is.Not.EqualTo(tc.Digest), nameof(newTc.Digest));
            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            Assert.That(newTc.FunctionName, Is.EqualTo(tc.FunctionName), nameof(newTc.FunctionName));
            if (hexCustomization)
            {
                Assert.That(newTc.CustomizationHex, Is.EqualTo(tc.CustomizationHex), nameof(newTc.CustomizationHex));
                Assert.That(newTc.Customization, Is.Not.EqualTo(tc.Customization), nameof(newTc.Customization));
            }
            else
            {
                Assert.That(newTc.CustomizationHex, Is.Not.EqualTo(tc.CustomizationHex), nameof(newTc.CustomizationHex));
                Assert.That(newTc.Customization, Is.EqualTo(tc.Customization), nameof(newTc.Customization));
            }

            if (testType == "aft")
            {
                Assert.That(newTc.DigestLength, Is.EqualTo(tc.DigestLength), nameof(newTc.DigestLength));
            }
            else
            {
                Assert.That(newTc.DigestLength, Is.Not.EqualTo(tc.DigestLength), nameof(newTc.DigestLength));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
