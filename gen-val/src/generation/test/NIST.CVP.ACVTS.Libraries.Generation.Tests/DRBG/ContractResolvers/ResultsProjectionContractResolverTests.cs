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

            Assert.That(newTg.DerFunc, Is.Not.EqualTo(tg.DerFunc), nameof(newTg.DerFunc));
            Assert.That(newTg.PredResistance, Is.Not.EqualTo(tg.PredResistance), nameof(newTg.PredResistance));
            Assert.That(newTg.ReSeed, Is.Not.EqualTo(tg.ReSeed), nameof(newTg.ReSeed));
            Assert.That(newTg.EntropyInputLen, Is.Not.EqualTo(tg.EntropyInputLen), nameof(newTg.EntropyInputLen));
            Assert.That(newTg.NonceLen, Is.Not.EqualTo(tg.NonceLen), nameof(newTg.NonceLen));
            Assert.That(newTg.PersoStringLen, Is.Not.EqualTo(tg.PersoStringLen), nameof(newTg.PersoStringLen));
            Assert.That(newTg.AdditionalInputLen, Is.Not.EqualTo(tg.AdditionalInputLen), nameof(newTg.AdditionalInputLen));
            Assert.That(newTg.ReturnedBitsLen, Is.Not.EqualTo(tg.ReturnedBitsLen), nameof(newTg.ReturnedBitsLen));
        }

        /// <summary>
        /// Only includes the test case Id and the returnedBits
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
            Assert.That(newTc.ReturnedBits, Is.EqualTo(tc.ReturnedBits), nameof(newTc.ReturnedBits));

            Assert.That(newTc.EntropyInput, Is.Not.EqualTo(tc.EntropyInput), nameof(newTc.EntropyInput));
            Assert.That(newTc.Nonce, Is.Not.EqualTo(tc.Nonce), nameof(newTc.Nonce));
            Assert.That(newTc.PersoString, Is.Not.EqualTo(tc.PersoString), nameof(newTc.PersoString));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
