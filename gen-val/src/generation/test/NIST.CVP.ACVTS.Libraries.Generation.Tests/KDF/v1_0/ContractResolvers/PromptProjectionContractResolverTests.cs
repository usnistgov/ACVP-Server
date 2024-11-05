using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.v1_0.ContractResolvers
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

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.KdfMode, Is.EqualTo(tg.KdfMode), nameof(newTg.KdfMode));
            Assert.That(newTg.MacMode, Is.EqualTo(tg.MacMode), nameof(newTg.MacMode));
            Assert.That(newTg.CounterLocation, Is.EqualTo(tg.CounterLocation), nameof(newTg.CounterLocation));

            if (kdfMode == KdfModes.Feedback)
            {
                Assert.That(newTg.ZeroLengthIv, Is.EqualTo(tg.ZeroLengthIv), nameof(newTg.ZeroLengthIv));
            }

            if (counterLocation != CounterLocations.None)
            {
                Assert.That(newTg.CounterLength, Is.EqualTo(tg.CounterLength), nameof(newTg.CounterLength));
            }
            else
            {
                Assert.That(newTg.CounterLength, Is.Not.EqualTo(tg.CounterLength), nameof(newTg.CounterLength));
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

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.KeyIn, Is.EqualTo(tc.KeyIn), nameof(newTc.KeyIn));

            if (kdfMode == KdfModes.Feedback)
            {
                Assert.That(newTc.IV, Is.EqualTo(tc.IV), nameof(newTc.IV));
            }
            else
            {
                Assert.That(newTc.IV, Is.Not.EqualTo(tc.IV), nameof(newTc.IV));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regexTestPassed.Matches(json).Count == 0, Is.True);
        }
    }
}
