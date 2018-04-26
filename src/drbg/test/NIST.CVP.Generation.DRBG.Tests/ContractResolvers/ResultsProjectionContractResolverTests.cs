using System;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.DRBG.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests.ContractResolvers
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

            Assert.AreNotEqual(tg.DerFunc, newTg.DerFunc, nameof(newTg.DerFunc));
            Assert.AreNotEqual(tg.PredResistance, newTg.PredResistance, nameof(newTg.PredResistance));
            Assert.AreNotEqual(tg.ReSeed, newTg.ReSeed, nameof(newTg.ReSeed));
            Assert.AreNotEqual(tg.EntropyInputLen, newTg.EntropyInputLen, nameof(newTg.EntropyInputLen));
            Assert.AreNotEqual(tg.NonceLen, newTg.NonceLen, nameof(newTg.NonceLen));
            Assert.AreNotEqual(tg.PersoStringLen, newTg.PersoStringLen, nameof(newTg.PersoStringLen));
            Assert.AreNotEqual(tg.AdditionalInputLen, newTg.AdditionalInputLen, nameof(newTg.AdditionalInputLen));
            Assert.AreNotEqual(tg.ReturnedBitsLen, newTg.ReturnedBitsLen, nameof(newTg.ReturnedBitsLen));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.ReturnedBits, newTc.ReturnedBits, nameof(newTc.ReturnedBits));
            
            Assert.AreNotEqual(tc.EntropyInput, newTc.EntropyInput, nameof(newTc.EntropyInput));
            Assert.AreNotEqual(tc.Nonce, newTc.Nonce, nameof(newTc.Nonce));
            Assert.AreNotEqual(tc.PersoString, newTc.PersoString, nameof(newTc.PersoString));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
