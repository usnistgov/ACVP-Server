using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.DRBG.v1_0;
using NIST.CVP.Generation.DRBG.v1_0.ContractResolvers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests.ContractResolvers
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
            Assert.AreEqual(tg.DerFunc, newTg.DerFunc, nameof(newTg.DerFunc));
            Assert.AreEqual(tg.PredResistance, newTg.PredResistance, nameof(newTg.PredResistance));
            Assert.AreEqual(tg.ReSeed, newTg.ReSeed, nameof(newTg.ReSeed));
            Assert.AreEqual(tg.EntropyInputLen, newTg.EntropyInputLen, nameof(newTg.EntropyInputLen));
            Assert.AreEqual(tg.NonceLen, newTg.NonceLen, nameof(newTg.NonceLen));
            Assert.AreEqual(tg.PersoStringLen, newTg.PersoStringLen, nameof(newTg.PersoStringLen));
            Assert.AreEqual(tg.AdditionalInputLen, newTg.AdditionalInputLen, nameof(newTg.AdditionalInputLen));
            Assert.AreEqual(tg.ReturnedBitsLen, newTg.ReturnedBitsLen, nameof(newTg.ReturnedBitsLen));
            Assert.AreEqual(tg.Mode, newTg.Mode, nameof(newTg.Mode));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            Assert.AreNotEqual(tc.ReturnedBits, newTc.ReturnedBits, nameof(newTc.ReturnedBits));

            Assert.AreEqual(tc.EntropyInput, newTc.EntropyInput, nameof(newTc.EntropyInput));
            Assert.AreEqual(tc.Nonce, newTc.Nonce, nameof(newTc.Nonce));
            Assert.AreEqual(tc.PersoString, newTc.PersoString, nameof(newTc.PersoString));
            for (int i = 0; i < tc.OtherInput.Count; i++)
            {
                var otherInput = tc.OtherInput[i];

                Assert.AreEqual(otherInput.AdditionalInput, newTc.OtherInput[i].AdditionalInput, nameof(otherInput.AdditionalInput));
                Assert.AreEqual(otherInput.EntropyInput, newTc.OtherInput[i].EntropyInput, nameof(otherInput.EntropyInput));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
