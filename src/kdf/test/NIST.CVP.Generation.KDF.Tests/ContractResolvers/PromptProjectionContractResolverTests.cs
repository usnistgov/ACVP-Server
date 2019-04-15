using System.Text.RegularExpressions;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.KDF.v1_0;
using NIST.CVP.Generation.KDF.v1_0.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KDF.Tests.ContractResolvers
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

        [Test]
        [TestCase(KdfModes.Counter, CounterLocations.None)]
        [TestCase(KdfModes.Feedback, CounterLocations.None)]
        [TestCase(KdfModes.Counter, CounterLocations.AfterFixedData)]
        [TestCase(KdfModes.Feedback, CounterLocations.AfterFixedData)]
        public void ShouldSerializeGroupProperties(KdfModes kdfMode, CounterLocations counterLocation)
        {
            var tvs = TestDataMother.GetTestGroups(1, kdfMode, counterLocation);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.KdfMode, newTg.KdfMode, nameof(newTg.KdfMode));
            Assert.AreEqual(tg.MacMode, newTg.MacMode, nameof(newTg.MacMode));
            Assert.AreEqual(tg.CounterLocation, newTg.CounterLocation, nameof(newTg.CounterLocation));

            if (kdfMode == KdfModes.Feedback)
            {
                Assert.AreEqual(tg.ZeroLengthIv, newTg.ZeroLengthIv, nameof(newTg.ZeroLengthIv));
            }

            if (counterLocation != CounterLocations.None)
            {
                Assert.AreEqual(tg.CounterLength, newTg.CounterLength, nameof(newTg.CounterLength));
            }
            else
            {
                Assert.AreNotEqual(tg.CounterLength, newTg.CounterLength, nameof(newTg.CounterLength));
            }
        }

        [Test]
        [TestCase(KdfModes.Counter, CounterLocations.None)]
        [TestCase(KdfModes.Feedback, CounterLocations.None)]
        [TestCase(KdfModes.Counter, CounterLocations.AfterFixedData)]
        [TestCase(KdfModes.Feedback, CounterLocations.AfterFixedData)]
        public void ShouldSerializeEncryptCaseProperties(KdfModes kdfMode, CounterLocations counterLocation)
        {
            var tvs = TestDataMother.GetTestGroups(1, kdfMode, counterLocation);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];
            
            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.KeyIn, newTc.KeyIn, nameof(newTc.KeyIn));

            if (kdfMode == KdfModes.Feedback)
            {
                Assert.AreEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            }
            else
            {
                Assert.AreNotEqual(tc.IV, newTc.IV, nameof(newTc.IV));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
        }
    }
}