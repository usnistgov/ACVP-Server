using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGGen.ContractResolvers
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

            if (pqGenMode == PrimeGenMode.None)
            {
                Regex regex = new Regex(nameof(TestGroup.PQGenMode), RegexOptions.IgnoreCase);
                Assert.That(regex.Matches(json).Count == 0, Is.True);
            }
            else
            {
                Assert.That(newTg.PQGenMode, Is.EqualTo(tg.PQGenMode), nameof(tg.PQGenMode));
            }

            if (gGenMode == GeneratorGenMode.None)
            {
                Regex regex = new Regex(nameof(TestGroup.GGenMode), RegexOptions.IgnoreCase);
                Assert.That(regex.Matches(json).Count == 0, Is.True);
            }
            else
            {
                Assert.That(newTg.GGenMode, Is.EqualTo(tg.GGenMode), nameof(tg.GGenMode));
            }
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

            if (gGenMode != GeneratorGenMode.None)
            {
                Assert.That(newTc.P, Is.EqualTo(tc.P), nameof(newTc.P));
                Assert.That(newTc.Q, Is.EqualTo(tc.Q), nameof(newTc.Q));
            }
            else
            {
                Assert.That(newTc.P, Is.Not.EqualTo(tc.P), nameof(newTc.P));
                Assert.That(newTc.Q, Is.Not.EqualTo(tc.Q), nameof(newTc.Q));
            }

            if (gGenMode == GeneratorGenMode.Canonical)
            {
                Assert.That(newTc.DomainSeed, Is.EqualTo(tc.DomainSeed), nameof(newTc.DomainSeed));
                Assert.That(newTc.Index, Is.EqualTo(tc.Index), nameof(newTc.Index));
            }
            else
            {
                Assert.That(newTc.DomainSeed, Is.Not.EqualTo(tc.DomainSeed), nameof(newTc.DomainSeed));
                Assert.That(newTc.Index, Is.Not.EqualTo(tc.Index), nameof(newTc.Index));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
