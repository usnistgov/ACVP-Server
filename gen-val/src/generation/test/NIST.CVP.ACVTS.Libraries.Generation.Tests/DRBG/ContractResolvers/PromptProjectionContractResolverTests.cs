using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DRBG.ContractResolvers
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
            Assert.That(newTg.DerFunc, Is.EqualTo(tg.DerFunc), nameof(newTg.DerFunc));
            Assert.That(newTg.PredResistance, Is.EqualTo(tg.PredResistance), nameof(newTg.PredResistance));
            Assert.That(newTg.ReSeed, Is.EqualTo(tg.ReSeed), nameof(newTg.ReSeed));
            Assert.That(newTg.EntropyInputLen, Is.EqualTo(tg.EntropyInputLen), nameof(newTg.EntropyInputLen));
            Assert.That(newTg.NonceLen, Is.EqualTo(tg.NonceLen), nameof(newTg.NonceLen));
            Assert.That(newTg.PersoStringLen, Is.EqualTo(tg.PersoStringLen), nameof(newTg.PersoStringLen));
            Assert.That(newTg.AdditionalInputLen, Is.EqualTo(tg.AdditionalInputLen), nameof(newTg.AdditionalInputLen));
            Assert.That(newTg.ReturnedBitsLen, Is.EqualTo(tg.ReturnedBitsLen), nameof(newTg.ReturnedBitsLen));
            Assert.That(newTg.Mode, Is.EqualTo(tg.Mode), nameof(newTg.Mode));
        }

        /// <summary>
        /// Include everything but the answer (returnedBits)
        /// </summary>
        [Test]
        public void ShouldSerializeCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            Assert.That(newTc.ReturnedBits, Is.Not.EqualTo(tc.ReturnedBits), nameof(newTc.ReturnedBits));

            Assert.That(newTc.EntropyInput, Is.EqualTo(tc.EntropyInput), nameof(newTc.EntropyInput));
            Assert.That(newTc.Nonce, Is.EqualTo(tc.Nonce), nameof(newTc.Nonce));
            Assert.That(newTc.PersoString, Is.EqualTo(tc.PersoString), nameof(newTc.PersoString));
            for (int i = 0; i < tc.OtherInput.Count; i++)
            {
                var otherInput = tc.OtherInput[i];

                Assert.That(newTc.OtherInput[i].AdditionalInput, Is.EqualTo(otherInput.AdditionalInput), nameof(otherInput.AdditionalInput));
                Assert.That(newTc.OtherInput[i].EntropyInput, Is.EqualTo(otherInput.EntropyInput), nameof(otherInput.EntropyInput));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
