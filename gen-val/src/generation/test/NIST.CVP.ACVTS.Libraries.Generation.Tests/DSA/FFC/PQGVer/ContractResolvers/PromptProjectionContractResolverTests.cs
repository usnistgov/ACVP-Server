using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGVer.ContractResolvers
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
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Probable)]
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Provable)]
        [TestCase(GeneratorGenMode.Canonical, PrimeGenMode.None)]
        [TestCase(GeneratorGenMode.Unverifiable, PrimeGenMode.None)]
        public void ShouldSerializeGroupProperties(GeneratorGenMode gGenMode, PrimeGenMode pqGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(1, gGenMode, pqGenMode);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.L, Is.EqualTo(tg.L), nameof(newTg.L));
            Assert.That(newTg.N, Is.EqualTo(tg.N), nameof(newTg.N));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));

            Assert.That(newTg.PQGenMode, Is.EqualTo(tg.PQGenMode), nameof(pqGenMode));
            Assert.That(newTg.GGenMode, Is.EqualTo(tg.GGenMode), nameof(gGenMode));
        }

        [Test]
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Probable)]
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Provable)]
        [TestCase(GeneratorGenMode.Canonical, PrimeGenMode.None)]
        [TestCase(GeneratorGenMode.Unverifiable, PrimeGenMode.None)]
        public void ShouldSerializeCaseProperties(GeneratorGenMode gGenMode, PrimeGenMode pqGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(1, gGenMode, pqGenMode);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            Assert.That(newTc.P, Is.EqualTo(tc.P), nameof(newTc.P));
            Assert.That(newTc.Q, Is.EqualTo(tc.Q), nameof(newTc.Q));

            if (pqGenMode != PrimeGenMode.None)
            {
                Assert.That(newTc.DomainSeed, Is.EqualTo(tc.DomainSeed), nameof(newTc.DomainSeed));
                Assert.That(newTc.DomainSeed.BitLength, Is.EqualTo(tg.N), "BitLength must be valid");
            }

            if (pqGenMode == PrimeGenMode.Provable)
            {
                Assert.That(newTc.PCount, Is.EqualTo(tc.PCount), nameof(newTc.PCount));
                Assert.That(newTc.QCount, Is.EqualTo(tc.QCount), nameof(newTc.QCount));
                Assert.That(newTc.PSeed, Is.EqualTo(tc.PSeed), nameof(newTc.PSeed));
                Assert.That(newTc.QSeed, Is.EqualTo(tc.QSeed), nameof(newTc.QSeed));
                Assert.That(newTc.PSeed.BitLength, Is.EqualTo(tg.N), "BitLength must be valid");
                Assert.That(newTc.QSeed.BitLength, Is.EqualTo(tg.N), "BitLength must be valid");
            }
            else
            {
                Assert.That(newTc.PCount, Is.Not.EqualTo(tc.PCount), nameof(newTc.PCount));
                Assert.That(newTc.QCount, Is.Not.EqualTo(tc.QCount), nameof(newTc.QCount));
                Assert.That(newTc.PSeed, Is.Not.EqualTo(tc.PSeed), nameof(newTc.PSeed));
                Assert.That(newTc.QSeed, Is.Not.EqualTo(tc.QSeed), nameof(newTc.QSeed));
            }

            if (gGenMode != GeneratorGenMode.None)
            {
                Assert.That(newTc.G, Is.EqualTo(tc.G), nameof(newTc.G));
            }
            else
            {
                Assert.That(newTc.G, Is.Not.EqualTo(tc.G), nameof(newTc.G));
            }

            if (gGenMode == GeneratorGenMode.Canonical)
            {
                Assert.That(newTc.Index, Is.EqualTo(tc.Index), nameof(newTc.Index));
            }

            if (gGenMode == GeneratorGenMode.Unverifiable)
            {
                Assert.That(newTc.H, Is.EqualTo(tc.H), nameof(newTc.H));
                Assert.That(newTc.DomainSeed, Is.EqualTo(tc.DomainSeed), nameof(newTc.DomainSeed));
                Assert.That(newTc.DomainSeed.BitLength, Is.EqualTo(tg.N), "BitLength must be valid");   // Uses a probable seed
            }
            else
            {
                Assert.That(newTc.H, Is.Not.EqualTo(tc.H), nameof(newTc.H));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
