using System.Text.RegularExpressions;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.DSA.FFC.PQGVer.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.L, newTg.L, nameof(newTg.L));
            Assert.AreEqual(tg.N, newTg.N, nameof(newTg.N));
            Assert.AreEqual(tg.HashAlgName, newTg.HashAlgName, nameof(newTg.HashAlgName));

            Assert.AreEqual(tg.PQGenMode, newTg.PQGenMode, nameof(pqGenMode));
            Assert.AreEqual(tg.GGenMode, newTg.GGenMode, nameof(gGenMode));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            Assert.AreEqual(tc.P, newTc.P, nameof(newTc.P));
            Assert.AreEqual(tc.Q, newTc.Q, nameof(newTc.Q));

            if (pqGenMode == PrimeGenMode.Probable)
            {
                Assert.AreEqual(tc.DomainSeed, newTc.DomainSeed, nameof(newTc.DomainSeed));
                Assert.AreEqual(tc.Index, newTc.Index, nameof(newTc.Index));
            }

            if (pqGenMode == PrimeGenMode.Provable)
            {
                Assert.AreEqual(tc.PCount, newTc.PCount, nameof(newTc.PCount));
                Assert.AreEqual(tc.QCount, newTc.QCount, nameof(newTc.QCount));
                Assert.AreEqual(tc.PSeed, newTc.PSeed, nameof(newTc.PSeed));
                Assert.AreEqual(tc.QSeed, newTc.QSeed, nameof(newTc.QSeed));
            }
            else
            {
                Assert.AreNotEqual(tc.PCount, newTc.PCount, nameof(newTc.PCount));
                Assert.AreNotEqual(tc.QCount, newTc.QCount, nameof(newTc.QCount));
                Assert.AreNotEqual(tc.PSeed, newTc.PSeed, nameof(newTc.PSeed));
                Assert.AreNotEqual(tc.QSeed, newTc.QSeed, nameof(newTc.QSeed));
            }

            if (gGenMode != GeneratorGenMode.None)
            {
                Assert.AreEqual(tc.G, newTc.G, nameof(newTc.G));
            }
            else
            {
                Assert.AreNotEqual(tc.G, newTc.G, nameof(newTc.G));
            }

            if (gGenMode == GeneratorGenMode.Canonical)
            {
                Assert.AreEqual(tc.Index, newTc.Index, nameof(newTc.Index));
            }
           
            if (gGenMode == GeneratorGenMode.Unverifiable)
            {
                Assert.AreEqual(tc.H, newTc.H, nameof(newTc.H));
                Assert.AreEqual(tc.DomainSeed, newTc.DomainSeed, nameof(newTc.DomainSeed));
            }
            else
            {
                Assert.AreNotEqual(tc.H, newTc.H, nameof(newTc.H));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
