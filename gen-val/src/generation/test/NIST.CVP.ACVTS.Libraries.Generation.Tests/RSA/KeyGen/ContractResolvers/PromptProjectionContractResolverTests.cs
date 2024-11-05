using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.KeyGen.ContractResolvers
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
        [TestCase(PrimeGenFips186_4Modes.B32)]
        [TestCase(PrimeGenFips186_4Modes.B33)]
        [TestCase(PrimeGenFips186_4Modes.B34)]
        [TestCase(PrimeGenFips186_4Modes.B35)]
        [TestCase(PrimeGenFips186_4Modes.B36)]
        public void ShouldSerializeGroupProperties(PrimeGenFips186_4Modes primeGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(primeGenMode);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            // Always include
            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.Modulo, Is.EqualTo(tg.Modulo), nameof(newTg.Modulo));
            Assert.That(newTg.FixedPubExp, Is.EqualTo(tg.FixedPubExp), nameof(newTg.FixedPubExp));
            Assert.That(newTg.PubExp, Is.EqualTo(tg.PubExp), nameof(newTg.PubExp));
            Assert.That(newTg.InfoGeneratedByServer, Is.EqualTo(tg.InfoGeneratedByServer), nameof(newTg.InfoGeneratedByServer));
            Assert.That(newTg.KeyFormat, Is.EqualTo(tg.KeyFormat), nameof(newTg.KeyFormat));
            Assert.That(newTg.PrimeGenMode, Is.EqualTo(tg.PrimeGenMode), nameof(newTg.PrimeGenMode));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));

            // Provable
            if (primeGenMode == PrimeGenFips186_4Modes.B32 || primeGenMode == PrimeGenFips186_4Modes.B34 ||
                primeGenMode == PrimeGenFips186_4Modes.B35)
            {
                Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            }

            // Probable
            if (primeGenMode == PrimeGenFips186_4Modes.B33 || primeGenMode == PrimeGenFips186_4Modes.B35 ||
                primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.That(newTg.PrimeTest, Is.EqualTo(tg.PrimeTest), nameof(newTg.PrimeTest));
            }
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        [Test]
        [TestCase(PrimeGenFips186_4Modes.B32)]
        [TestCase(PrimeGenFips186_4Modes.B33)]
        [TestCase(PrimeGenFips186_4Modes.B34)]
        [TestCase(PrimeGenFips186_4Modes.B35)]
        [TestCase(PrimeGenFips186_4Modes.B36)]
        public void ShouldSerializeCaseProperties(PrimeGenFips186_4Modes primeGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(primeGenMode);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            // Always include
            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Deferred, Is.EqualTo(tc.Deferred), nameof(newTc.Deferred));

            // Not probable
            if (primeGenMode != PrimeGenFips186_4Modes.B33)
            {
                Assert.That(newTc.E, Is.EqualTo(tc.E), nameof(newTc.E));
            }

            // Provable
            if (primeGenMode == PrimeGenFips186_4Modes.B32 || primeGenMode == PrimeGenFips186_4Modes.B34 ||
                primeGenMode == PrimeGenFips186_4Modes.B35)
            {
                Assert.That(newTc.Seed, Is.EqualTo(tc.Seed), nameof(newTc.Seed));
            }

            // With Aux
            if (primeGenMode == PrimeGenFips186_4Modes.B34 || primeGenMode == PrimeGenFips186_4Modes.B35 ||
                primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.That(newTc.Bitlens, Is.EqualTo(tc.Bitlens), nameof(newTc.Bitlens));
            }

            // Probable With Aux
            if (primeGenMode == PrimeGenFips186_4Modes.B35 || primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.That(newTc.XP, Is.EqualTo(tc.XP), nameof(newTc.XP));
                Assert.That(newTc.XQ, Is.EqualTo(tc.XQ), nameof(newTc.XQ));
            }

            // Probable With Probable Aux
            if (primeGenMode == PrimeGenFips186_4Modes.B36)
            {
                Assert.That(newTc.XP1, Is.EqualTo(tc.XP1), nameof(newTc.XP1));
                Assert.That(newTc.XP2, Is.EqualTo(tc.XP2), nameof(newTc.XP2));

                Assert.That(newTc.XQ1, Is.EqualTo(tc.XQ1), nameof(newTc.XQ1));
                Assert.That(newTc.XQ2, Is.EqualTo(tc.XQ2), nameof(newTc.XQ2));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
